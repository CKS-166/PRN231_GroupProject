using AutoMapper;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.ProjectDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectDonate;
using FPTU_Starter.Application.ViewModel.ProjectDTO.ProjectPackageDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.RewardItemDTO;
using FPTU_Starter.Application.ViewModel.ProjectDTO.SubCategoryPrj;
using FPTU_Starter.Application.ViewModel.TransactionDTO;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FPTU_Starter.Domain.Enum.ProjectEnum;

namespace FPTU_Starter.Application.Services
{
    public class ProjectManagementService : IProjectManagementService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IWalletService _walletService;
        private readonly IUserManagementService _userManagement;
        private readonly IPackageManagementService _packageManagement;
        private ClaimsPrincipal _claimsPrincipal;
        private IInteractionService _interactionService;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILikeRepository _likeRepository;

        public ProjectManagementService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IWalletService walletService,
            IUserManagementService userManagement,
            IPackageManagementService packageManagement,
            UserManager<ApplicationUser> userManager, IInteractionService interactionService,
            ILikeRepository likeRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _walletService = walletService;
            _userManagement = userManagement;
            _packageManagement = packageManagement;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            _userManager = userManager;
            _interactionService = interactionService;
            _likeRepository = likeRepository;
        }

        public async Task<ResultDTO<string>> CreateProject(ProjectAddRequest projectAddRequest)
        {
            try
            {
                ApplicationUser owner = _unitOfWork.UserRepository.Get(p => p.Email == projectAddRequest.ProjectOwnerEmail);
                List<SubCategory> subCates = new List<SubCategory>();
                foreach (SubCatePrjAddRequest sa in projectAddRequest.SubCategories)
                {
                    SubCategory sub = _unitOfWork.SubCategoryRepository.Get(sc => sc.Id == sa.Id);
                    subCates.Add(sub);
                }
                foreach (PackageAddRequest pack in projectAddRequest.Packages)
                {
                    if (pack.RequiredAmount < 5000)
                    {
                        return ResultDTO<string>.Fail("Price for package must be at least 5000");
                    }
                }
                Project project = _mapper.Map<Project>(projectAddRequest);
                project.SubCategories = subCates;
                project.ProjectOwner = owner;
                project.CreatedDate = DateTime.Now;
                project.ProjectStatus = ProjectStatus.Pending;
                //create free package
                ProjectPackage freePackage = new ProjectPackage
                {
                    Id = Guid.NewGuid(),
                    PackageImage = "",
                    PackageDescription = "",
                    LimitQuantity = 0,
                    PackageType = "Free"
                };
                project.Packages.Add(freePackage);
                await _unitOfWork.ProjectRepository.AddAsync(project);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Add Sucessfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ResultDTO<List<ProjectViewResponse>>> ViewAllProjectsAsync()
        {
            try
            {
                IEnumerable<Project> projects = await _unitOfWork.ProjectRepository.GetQueryable()
                    .Include(p => p.Packages).ThenInclude(pa => pa.RewardItems)
                    .Include(p => p.ProjectOwner)
                    //.Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.SubCategories)
                    .Include(p => p.BankAccount)
                    .ToListAsync();
                IEnumerable<ProjectViewResponse> responses = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectViewResponse>>(projects);

                return ResultDTO<List<ProjectViewResponse>>.Success(responses.ToList(), "");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<ResultDTO<string>> UpdateProjectStatus(Guid id, ProjectStatus projectStatus)
        {
            try
            {
                Project project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
                project.ProjectStatus = projectStatus;
                _unitOfWork.ProjectRepository.Update(project);
                await _unitOfWork.CommitAsync();

                return ResultDTO<string>.Success("", "Update Sucessfully");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ResultDTO<ProjectViewResponse>> GetProjectById(Guid id)
        {
            try
            {
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
                var projectDto = _mapper.Map<ProjectViewResponse>(project);
                return ResultDTO<ProjectViewResponse>.Success(projectDto);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<ResultDTO<List<ProjectViewResponse>>> GetUserProjects(string searchType, string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName)
        {
            try
            {
                IQueryable<Project> projectQuery = _unitOfWork.ProjectRepository.GetQueryable()
                    .AsNoTracking()
                    .Include(p => p.Packages).ThenInclude(pa => pa.RewardItems)
                    .Include(p => p.ProjectOwner)
                    .Include(p => p.SubCategories).ThenInclude(s => s.Category)
                    .Include(p => p.Images)
                    .Include(p => p.BankAccount);

                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {

                }
                else
                {
                    var userEmailClaims = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                    if (userEmailClaims == null)
                    {
                        return ResultDTO<List<ProjectViewResponse>>.Fail("User not found.");
                    }
                    var userEmail = userEmailClaims.Value;
                    var applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);

                    if (searchType == "user")
                    {
                        projectQuery = projectQuery.Where(x => x.ProjectOwner.Id == applicationUser.Id);
                    }
                }

                // Apply filters before executing the query
                if (!string.IsNullOrEmpty(searchName))
                {
                    projectQuery = projectQuery.Where(x => x.ProjectName.ToLower().Contains(searchName.ToLower()));
                }

                if (moneyTarget.HasValue)
                {
                    switch (moneyTarget.Value)
                    {
                        case 1:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 0 && x.ProjectTarget < 1000000);
                            break;
                        case 2:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 1000000 && x.ProjectTarget < 10000000);
                            break;
                        case 3:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 10000000 && x.ProjectTarget < 100000000);
                            break;
                        case 4:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 100000000);
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    projectQuery = projectQuery.Where(x => x.SubCategories.Any(s => s.Category.Name.ToLower().Contains(categoryName.ToLower())));
                }

                if (projectStatus.HasValue)
                {
                    projectQuery = projectQuery.Where(x => x.ProjectStatus == projectStatus.Value);
                }

                var projectList = await projectQuery.ToListAsync();

                var responses = _mapper.Map<List<ProjectViewResponse>>(projectList);
                return ResultDTO<List<ProjectViewResponse>>.Success(responses, "");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ResultDTO<List<ProjectViewResponse>>> GetAllProjects(string? searchName, ProjectStatus? projectStatus, int? moneyTarget, string? categoryName)
        {
            try
            {
                IQueryable<Project> projectQuery = _unitOfWork.ProjectRepository.GetQueryable()
                    .AsNoTracking()
                    .Include(p => p.Packages).ThenInclude(pa => pa.RewardItems)
                    .Include(p => p.ProjectOwner)
                    .Include(p => p.SubCategories).ThenInclude(s => s.Category)
                    .Include(p => p.Images)
                    .Where(p => p.ProjectStatus == ProjectStatus.Successful || p.ProjectStatus == ProjectStatus.Processing || p.ProjectStatus == ProjectStatus.Withdrawed);

                // Apply filters before executing the query
                if (!string.IsNullOrEmpty(searchName))
                {
                    projectQuery = projectQuery.Where(x => x.ProjectName.ToLower().Contains(searchName.ToLower()));
                }

                if (moneyTarget.HasValue)
                {
                    switch (moneyTarget.Value)
                    {
                        case 1:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 0 && x.ProjectTarget < 1000000);
                            break;
                        case 2:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 1000000 && x.ProjectTarget < 10000000);
                            break;
                        case 3:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 10000000 && x.ProjectTarget < 100000000);
                            break;
                        case 4:
                            projectQuery = projectQuery.Where(x => x.ProjectTarget >= 100000000);
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    projectQuery = projectQuery.Where(x => x.SubCategories.Any(s => s.Category.Name.ToLower().Contains(categoryName.ToLower())));
                }

                if (projectStatus.HasValue)
                {
                    projectQuery = projectQuery.Where(x => x.ProjectStatus == projectStatus.Value);
                }

                var projectList = await projectQuery.ToListAsync();

                var responses = _mapper.Map<List<ProjectViewResponse>>(projectList);
                return ResultDTO<List<ProjectViewResponse>>.Success(responses, "");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ResultDTO<string>> UpdateProject(ProjectUpdateRequest request)
        {
            try
            {
                Project existedPrj = await _unitOfWork.ProjectRepository.GetByIdAsync(request.Id);
                if (existedPrj != null)
                {
                    List<ProjectImage> images = _mapper.Map<List<ProjectImage>>(request.Images);
                    //List<ProjectPackage> packages = _mapper.Map<List<ProjectPackage>>(request.Packages);
                    existedPrj.Images.Clear();
                    foreach (ProjectImage image in images)
                    {
                        image.ProjectId = existedPrj.Id;
                    }
                    _mapper.Map(request, existedPrj);
                    existedPrj.Images = images;
                    _unitOfWork.ProjectRepository.Update(existedPrj);
                    await _unitOfWork.CommitAsync();
                    return ResultDTO<string>.Success("Update Sucessfully");
                }
                else
                {
                    return ResultDTO<string>.Fail("Project Not Found", 404);
                }
            }
            catch (Exception e)
            {
                return ResultDTO<string>.Fail(e.Message, 500);

            }
        }

        public async Task<ResultDTO<string>> UpdatePackages(Guid id, List<PackageViewResponse> req)
        {
            try
            {
                List<ProjectPackage> nPack = new List<ProjectPackage>();
                foreach (PackageViewResponse response in req)
                {
                    ProjectPackage pack = _unitOfWork.PackageRepository.GetQueryable().Include(p => p.RewardItems).FirstOrDefault(p => p.Id == response.Id);
                    List<RewardItem> rewardItems = new List<RewardItem>();
                    nPack.Add(pack);
                    _mapper.Map(response, pack);
                    foreach (RewardItemViewResponse item in response.RewardItems)
                    {
                        RewardItem reward = _unitOfWork.RewardItemRepository.GetById(item.Id);
                        reward.Name = item.Name;
                        reward.Description = item.Description;
                        reward.Quantity = item.Quantity;
                        reward.ImageUrl = item.ImageUrl;
                        rewardItems.Add(reward);
                    }
                    pack.RewardItems = rewardItems;
                    _unitOfWork.PackageRepository.Update(pack);
                }
                _mapper.Map(req, nPack);
                //_unitOfWork.PackageRepository.UpdateRange(nPack);
                await _unitOfWork.CommitAsync();
                return ResultDTO<string>.Success("Update Sucessfully");
            }
            catch (Exception e)
            {
                return ResultDTO<string>.Fail(e.Message, 500);

            }

        }
        public async Task<ResultDTO<ProjectDonateResponse>> DonateProject(ProjectDonateRequest request)
        {
            try
            {
                var user = _userManagement.GetUserInfo().Result;
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var project = await _unitOfWork.ProjectRepository.GetAsync(x => x.Id.Equals(request.ProjectId));
                if (project is null)
                {
                    return ResultDTO<ProjectDonateResponse>.Fail("Project null");
                }
                //Check Project Status
                if (!project.ProjectStatus.Equals(ProjectEnum.ProjectStatus.Processing) &&
                    !project.ProjectStatus.Equals(ProjectEnum.ProjectStatus.Successful))
                {
                    return ResultDTO<ProjectDonateResponse>.Fail("Project cannot be donated to");
                }
                var userWallet = await _unitOfWork.WalletRepository.GetAsync(x => x.BackerId!.Equals(exitUser.Id));

                var IsEnoughMoney = await _walletService.CheckAccoutBallance(request.AmountDonate);
                if (IsEnoughMoney._isSuccess)
                {
                    // check enough money then allow to donate (minus the amount donation)
                    userWallet.Balance -= request.AmountDonate;
                    project.ProjectBalance += request.AmountDonate;
                    //check free package
                    var FreeDonate = await _unitOfWork.PackageRepository.GetAsync(x => x.ProjectId.Equals(project.Id) && x.PackageType.Equals("Free"));
                    //create a transaction
                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = userWallet.Id,
                        Wallet = exitUser.Wallet,
                        CreateDate = DateTime.Now,
                        Description = $"{exitUser.Name} has just donated project {project.ProjectName}",
                        TotalAmount = request.AmountDonate,
                        TransactionType = TransactionTypes.FreeDonation,
                        PackageId = FreeDonate.Id
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);

                    //savechange
                    await _unitOfWork.CommitAsync();

                    //custom response
                    var response = new ProjectDonateResponse
                    {
                        ProjectName = project.ProjectName,
                        DonateAmount = request.AmountDonate,
                        status = true

                    };
                    return ResultDTO<ProjectDonateResponse>.Success(response, "Successfully donate to this project");
                }
                return ResultDTO<ProjectDonateResponse>.Fail($"Something went wrong, {IsEnoughMoney._message}");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ResultDTO<ProjectDonateResponse>> PackageDonateProject(PackageDonateRequest request)
        {
            try
            {
                var user = _userManagement.GetUserInfo().Result;
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var package = await _unitOfWork.PackageRepository.GetAsync(x => x.Id.Equals(request.PackageId));
                if (package.LimitQuantity == 0)
                {
                    return ResultDTO<ProjectDonateResponse>.Fail("This package is out of quanity");
                }
                var project = await _unitOfWork.ProjectRepository.GetAsync(x => x.Id.Equals(package.ProjectId));
                var userWallet = await _unitOfWork.WalletRepository.GetAsync(x => x.BackerId!.Equals(exitUser.Id));

                if (package is null)
                {
                    return ResultDTO<ProjectDonateResponse>.Fail("Package can not free");
                }

                //Check Project Status
                if (!project.ProjectStatus.Equals(ProjectEnum.ProjectStatus.Processing) &&
                    !project.ProjectStatus.Equals(ProjectEnum.ProjectStatus.Successful))
                {
                    return ResultDTO<ProjectDonateResponse>.Fail("Project cannot be donated to");
                }


                var IsEnoughMoney = await _walletService.CheckAccoutBallance(package.RequiredAmount);
                if (IsEnoughMoney._isSuccess)
                {
                    // check enough money then allow to donate (minus the amount donation)
                    userWallet.Balance -= package.RequiredAmount;
                    project.ProjectBalance += package.RequiredAmount;
                    package.LimitQuantity -= 1;
                    //create a transaction
                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = userWallet.Id,
                        Wallet = exitUser.Wallet,
                        CreateDate = DateTime.Now,
                        Description = $"{exitUser.Name} has just donated project {project.ProjectName} with package",
                        TotalAmount = package.RequiredAmount,
                        TransactionType = TransactionTypes.PackageDonation,
                        PackageId = package.Id
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);

                    //savechange
                    await _unitOfWork.CommitAsync();

                    //custom response
                    var response = new ProjectDonateResponse
                    {
                        ProjectName = project.ProjectName,
                        DonateAmount = package.RequiredAmount,
                        status = true

                    };
                    return ResultDTO<ProjectDonateResponse>.Success(response, "Successfully donate to this project");
                }
                return ResultDTO<ProjectDonateResponse>.Fail($"Something went wrong");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<ResultDTO<string>> FailedProject()
        {
            try
            {
                List<Project> projects = _unitOfWork.ProjectRepository.GetAll().ToList();
                foreach (Project project in projects)
                {
                    DateTime today = DateTime.Today;
                    if (project.StartDate >= today)
                    {
                        if (project.ProjectStatus == ProjectStatus.Pending)
                        {
                            project.ProjectStatus = ProjectStatus.Failed;
                        }
                    }
                    await _unitOfWork.CommitAsync();
                }
                return ResultDTO<string>.Success("Project has been expired");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);

            }
        }

        public async Task<ResultDTO<List<ProjectDonateResponse>>> CountProjectDonate()
        {
            try
            {
                List<Transaction> trans = _unitOfWork.TransactionRepository.GetQueryable()
                    .Where(t => (t.TransactionType == TransactionTypes.FreeDonation
                    || t.TransactionType == TransactionTypes.PackageDonation)).ToList();

                Dictionary<Guid, int> count = new Dictionary<Guid, int>();
                foreach (Transaction transaction in trans)
                {
                    ProjectPackage package = _unitOfWork.PackageRepository.GetQueryable().Include(pa => pa.Project).FirstOrDefault(pa => pa.Id == transaction.PackageId);
                    Project project = _unitOfWork.ProjectRepository.GetById(package.ProjectId);
                    if (project != null)
                    {
                        if (count.ContainsKey(project.Id))
                        {
                            count[project.Id]++;
                        }
                        else
                        {
                            count[project.Id] = 1;
                        }
                    }
                }
                List<ProjectDonateResponse> result = count.Select(c => new ProjectDonateResponse
                {
                    ProjectName = _unitOfWork.ProjectRepository.GetQueryable().FirstOrDefault(p => p.Id == c.Key).ProjectName,
                    Count = c.Value
                }).ToList();
                return ResultDTO<List<ProjectDonateResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<List<ProjectViewResponse>>> GetProjectHomePage(int itemPerPage, int currentPage)
        {
            try
            {
                List<Project> allProjects = _unitOfWork.ProjectRepository.GetQueryable()
                    .Include(p => p.Packages).ThenInclude(pa => pa.RewardItems)
                    .Include(p => p.ProjectOwner)
                    .Include(p => p.SubCategories).ThenInclude(s => s.Category)
                    .Include(p => p.Images)
                    .Where(p => p.ProjectStatus == ProjectStatus.Processing || p.ProjectStatus == ProjectStatus.Successful || p.ProjectStatus == ProjectStatus.Withdrawed)
                    .ToList();

                List<Transaction> trans = _unitOfWork.TransactionRepository.GetQueryable()
                    .Where(t => (t.TransactionType == TransactionTypes.FreeDonation
                    || t.TransactionType == TransactionTypes.PackageDonation)).ToList();
                Dictionary<Guid, int> count = new Dictionary<Guid, int>();
                foreach (Transaction transaction in trans)
                {
                    ProjectPackage package = _unitOfWork.PackageRepository.GetQueryable().Include(pa => pa.Project).FirstOrDefault(pa => pa.Id == transaction.PackageId);
                    Project project = _unitOfWork.ProjectRepository.GetQueryable().FirstOrDefault(p => p.Id == package.ProjectId
                    && (p.ProjectStatus == ProjectStatus.Processing || p.ProjectStatus == ProjectStatus.Successful || p.ProjectStatus == ProjectStatus.Withdrawed));
                    if (project != null)
                    {
                        if (count.ContainsKey(project.Id))
                        {
                            count[project.Id]++;
                        }
                        else
                        {
                            count[project.Id] = 1;
                        }
                    }
                }
                List<Project> homeProjects = new List<Project>();
                List<Project> projects = new List<Project>();
                foreach (var item in count)
                {
                    Project project = _unitOfWork.ProjectRepository.GetQueryable().Include(p => p.Packages).ThenInclude(pa => pa.RewardItems)
                    .Include(p => p.ProjectOwner)
                    .Include(p => p.SubCategories).ThenInclude(s => s.Category)
                    .Include(p => p.Images)
                    .FirstOrDefault(p => p.Id == item.Key);
                    projects.Add(project);
                }
                foreach (Project prj in projects)
                {
                    homeProjects.Add(prj);
                }
                foreach (Project project in allProjects)
                {
                    if (!projects.Contains(project))
                    {
                        homeProjects.Add(project);
                    }
                }
                homeProjects = homeProjects.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToList();

                IEnumerable<ProjectViewResponse> responses = _mapper.Map<List<Project>, List<ProjectViewResponse>>(homeProjects);
                foreach (ProjectViewResponse projectView in responses)
                {
                    if (projectView != null)
                    {
                        List<TransactionBacker> backers = GetBackers(projectView.Id);
                        projectView.Backers = backers.Count;
                        var likes = _likeRepository.GetAll().Where(l => l.ProjectId == projectView.Id);
                        projectView.Likes = likes.ToList().Count;
                    }

                }
                return ResultDTO<List<ProjectViewResponse>>.Success(responses.ToList(), "");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ResultDTO<bool>> CheckHaveProject(Guid projectId)
        {
            try
            {
                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {
                    return ResultDTO<bool>.Success(false);
                }
                var userEmailClaims = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (userEmailClaims == null)
                {
                    return ResultDTO<bool>.Success(false);
                }
                var userEmail = userEmailClaims.Value;
                var applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);
                if (applicationUser == null)
                {
                    return ResultDTO<bool>.Success(false);
                }
                List<Project> projects = _unitOfWork.ProjectRepository.GetQueryable().Where(p => p.ProjectOwner.Email.Equals(applicationUser.Email)).ToList();

                if (projects.Count == 0)
                {
                    return ResultDTO<bool>.Success(false);
                }
                if (!projects.Contains(_unitOfWork.ProjectRepository.GetById(projectId)))
                {
                    return ResultDTO<bool>.Success(false);
                }
                return ResultDTO<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<bool>> CheckBackerProject(Guid projectId)
        {
            try
            {
                if (_claimsPrincipal == null || !_claimsPrincipal.Identity.IsAuthenticated)
                {
                    return ResultDTO<bool>.Success(false);
                }
                var userEmailClaims = _claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (userEmailClaims == null)
                {
                    return ResultDTO<bool>.Success(false);
                }
                var userEmail = userEmailClaims.Value;
                var applicationUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == userEmail);
                if (applicationUser == null)
                {
                    return ResultDTO<bool>.Success(false);
                }
                Wallet backerWallet = _unitOfWork.WalletRepository.GetQueryable().FirstOrDefault(w => w.Backer.Id == applicationUser.Id);
                Project project = _unitOfWork.ProjectRepository.GetQueryable().Include(p => p.Packages).FirstOrDefault(p => p.Id == projectId);
                foreach (ProjectPackage pack in project.Packages)
                {
                    if (_unitOfWork.TransactionRepository.GetQueryable().FirstOrDefault(t => t.WalletId == backerWallet.Id
                    && t.PackageId == pack.Id && (t.TransactionType == TransactionTypes.FreeDonation
                    || t.TransactionType == TransactionTypes.PackageDonation)) != null)
                    {
                        return ResultDTO<bool>.Success(true);
                    }
                }
                return ResultDTO<bool>.Success(false);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<List<TransactionBacker>>> GetProjectBackers(Guid projectId)
        {
            try
            {
                Project project = _unitOfWork.ProjectRepository.GetQueryable().Include(p => p.Packages).FirstOrDefault(p => p.Id == projectId);
                List<TransactionBacker> trans = new List<TransactionBacker>();
                foreach (ProjectPackage pack in project.Packages)
                {
                    List<Transaction> transactions = _unitOfWork.TransactionRepository.GetQueryable().Where(t => t.PackageId == pack.Id
                    && (t.TransactionType == TransactionTypes.FreeDonation || t.TransactionType == TransactionTypes.PackageDonation)).ToList();
                    foreach (Transaction transaction in transactions)
                    {
                        if (transaction != null)
                        {
                            Wallet backerWallet = _unitOfWork.WalletRepository.GetQueryable().Include(w => w.Backer).FirstOrDefault(w => w.Id == transaction.WalletId);
                            TransactionBacker transactionBacker = new TransactionBacker
                            {
                                Id = transaction.Id,
                                PackageId = transaction.PackageId,
                                TotalAmount = transaction.TotalAmount,
                                TransactionTypes = transaction.TransactionType == 0 ? "Package" : "Free",
                                CreateDate = transaction.CreateDate,
                                BackerName = backerWallet.Backer.AccountName,
                                BackerUrl = backerWallet.Backer.Avatar
                            };
                            trans.Add(transactionBacker);
                        }
                    }

                }
                return ResultDTO<List<TransactionBacker>>.Success(trans);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public List<TransactionBacker> GetBackers(Guid projectId)
        {
            Project project = _unitOfWork.ProjectRepository.GetQueryable().Include(p => p.Packages).FirstOrDefault(p => p.Id == projectId);
            List<TransactionBacker> trans = new List<TransactionBacker>();
            foreach (ProjectPackage pack in project.Packages)
            {
                Transaction transaction = _unitOfWork.TransactionRepository.GetQueryable().FirstOrDefault(t => t.PackageId == pack.Id
                && (t.TransactionType == TransactionTypes.FreeDonation || t.TransactionType == TransactionTypes.PackageDonation));
                if (transaction != null)
                {
                    Wallet backerWallet = _unitOfWork.WalletRepository.GetQueryable().Include(w => w.Backer).FirstOrDefault(w => w.Id == transaction.WalletId);
                    TransactionBacker transactionBacker = new TransactionBacker
                    {
                        Id = transaction.Id,
                        PackageId = transaction.PackageId,
                        TotalAmount = transaction.TotalAmount,
                        TransactionTypes = transaction.TransactionType == 0 ? "Package" : "Free",
                        CreateDate = transaction.CreateDate,
                        BackerName = backerWallet.Backer.AccountName,
                        BackerUrl = backerWallet.Backer.Avatar
                    };
                    trans.Add(transactionBacker);
                }

            }
            return trans;
        }
        public async Task<ResultDTO<int>> GetAllProject()
        {
            try
            {
                var numOfProject =  _unitOfWork.ProjectRepository.GetQueryable()
                        .Where(x=>x.ProjectStatus.Equals(ProjectEnum.ProjectStatus.Successful)).Count();
                return ResultDTO<int>.Success(numOfProject, "total of project is");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<decimal>> GetTotalMoney()
        {
            try
            {
                //total of project pending
                var listProject = _unitOfWork.ProjectRepository.GetQueryable();
                var totalSuccessfullMoney = listProject.Select(x => x.ProjectBalance).Sum(); // successfull nhung ma chua withdraw    

                //var list project successfull but withdraw. take from transaction
                var totalProjectCashOut = _unitOfWork.WithdrawRepository.GetQueryable()
                    .Where(x => x.RequestType == TransactionTypes.CashOut).Select(x => x.Amount).Sum();


                return ResultDTO<decimal>.Success(totalProjectCashOut + totalSuccessfullMoney, "total of money is");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<int>> GetAllPackages()
        {
            try
            {
                var listPackage = _unitOfWork.TransactionRepository.GetQueryable()
                    .Where(x => x.TransactionType == TransactionTypes.PackageDonation)
                    .Select(x => x.PackageId)
                    .Distinct()
                    .Count();

                return ResultDTO<int>.Success(listPackage, "total of packages is");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<List<ProjectSuccessRateDTO>>> GetProjectSuccessRate()
        {
            try
            {
                List<Project> allProjects = _unitOfWork.ProjectRepository.GetAll().ToList();
                List<ProjectSuccessRateDTO> results = new List<ProjectSuccessRateDTO>();
                foreach(Project project in allProjects)
                {
                    results.Add(new ProjectSuccessRateDTO
                    {
                        ProjectName = project.ProjectName,
                        SuccessRate = (decimal)project.ProjectBalance / project.ProjectTarget
                    });
                }
                return ResultDTO<List<ProjectSuccessRateDTO>>.Success(results);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}