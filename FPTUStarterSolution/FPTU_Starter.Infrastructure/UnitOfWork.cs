
using FPTU_Starter.Application;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Infrastructure.Database;
using FPTU_Starter.Infrastructure.Repository;

namespace FPTU_Starter.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _dbContext;
        private IUserRepository _userRepository;
        private IProjectRepository _projectRepository;
        private IPackageRepository _packageRepository;
        private ICategoryRepository _categoryRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private IWalletRepository _walletRepository;
        private IRewardItemRepository _rewardItemRepository;
        private ITransactionRepository _transactionRepository;
        private IAboutUsRepository _aboutUsRepository;
        private IWithdrawRepository _withdrawRepository;
        private IStageRepository _stageRepository;
        private ISystemWalletRepository _systemWalletRepository;
        private IBankAccountRepository _bankAccountRepository;
        public UnitOfWork(MyDbContext dbContext,
            IUserRepository UserRepository,
            IProjectRepository projectRepository,
            IPackageRepository packageRepository,
            ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IRewardItemRepository rewardItemRepository,
            IAboutUsRepository aboutUsRepository,
            IWithdrawRepository withdrawRepository,
            IStageRepository stageRepository,
            ISystemWalletRepository systemWalletRepository,
            IBankAccountRepository bankAccountRepository)
        {
            _dbContext = dbContext;
            _userRepository = UserRepository;
            _projectRepository = projectRepository;
            _packageRepository = packageRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _rewardItemRepository = rewardItemRepository;
            _aboutUsRepository = aboutUsRepository;
            _withdrawRepository = withdrawRepository;
            _stageRepository = stageRepository;
            _systemWalletRepository = systemWalletRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new UserRepository(_dbContext);
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                return _projectRepository = _projectRepository ?? new ProjectRepository(_dbContext);
            }
        }

        public IPackageRepository PackageRepository
        {
            get
            {
                return _packageRepository = _packageRepository ?? new PackageRepository(_dbContext);
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_dbContext);
            }
        }

        public ISubCategoryRepository SubCategoryRepository
        {
            get
            {
                return _subCategoryRepository = _subCategoryRepository ?? new SubCategoryRepository(_dbContext);
            }
        }

        public IWalletRepository WalletRepository
        {
            get
            {
                return _walletRepository = _walletRepository ?? new WalletRepository(_dbContext);
            }
        }

        public IRewardItemRepository RewardItemRepository
        {
            get
            {
                return _rewardItemRepository = _rewardItemRepository ?? new RewardItemRepository(_dbContext);
            }
        }
        public ITransactionRepository TransactionRepository
        {
            get
            {
                return _transactionRepository = _transactionRepository ?? new TransactionRepository(_dbContext);
            }
        }

        public IAboutUsRepository AboutUsRepository
        {
            get
            {
                return _aboutUsRepository = _aboutUsRepository ?? new AboutUsRepository(_dbContext);
            }
        }
        public IStageRepository StageRepository
        {
            get
            {
                return _stageRepository = _stageRepository ?? new StageRepository(_dbContext);
            }
        }

        public IWithdrawRepository WithdrawRepository
        {
            get
            {
                return _withdrawRepository = _withdrawRepository ?? new WithdrawRepository(_dbContext);
            }
        }

        public ISystemWalletRepository SystemWalletRepository
        {
            get
            {
                return _systemWalletRepository = _systemWalletRepository ?? new SystemWalletRepository(_dbContext);
            }
        }

        public IBankAccountRepository BankAccountRepository
        {
            get
            {
                return _bankAccountRepository = _bankAccountRepository ?? new BankAccountRepository(_dbContext);
            }
        }

        public void Commit()
             => _dbContext.SaveChanges();


        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();


        public void Rollback()
            => _dbContext.Dispose();


        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();
    }
}

