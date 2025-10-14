using Shopularity.Catalog.Domain.Categories;
using Shopularity.Catalog.Domain.Products;
using SixLabors.ImageSharp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Shopularity.Data;

public class ShopularityDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private const int DummyProductCount = 1000;

    private readonly ProductManager _productManager;
    private readonly IProductRepository _productRepository;
    private readonly CategoryManager _categoryManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWorkAccessor _unitOfWorkAccessor;
    private readonly CommentManager _commentManager;
    private readonly ICmsUserRepository _cmsUserRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IHttpClientFactory _clientFactory;

    public ShopularityDataSeedContributor(
        ProductManager productManager,
        IProductRepository productRepository,
        CategoryManager categoryManager,
        ICategoryRepository categoryRepository,
        IUnitOfWorkAccessor unitOfWorkAccessor,
        CommentManager commentManager,
        ICmsUserRepository cmsUserRepository,
        ICommentRepository commentRepository,
        IGuidGenerator guidGenerator,
        IdentityUserManager identityUserManager,
        IHttpClientFactory clientFactory)
    {
        _productManager = productManager;
        _productRepository = productRepository;
        _categoryManager = categoryManager;
        _categoryRepository = categoryRepository;
        _unitOfWorkAccessor = unitOfWorkAccessor;
        _commentManager = commentManager;
        _cmsUserRepository = cmsUserRepository;
        _commentRepository = commentRepository;
        _guidGenerator = guidGenerator;
        _identityUserManager = identityUserManager;
        _clientFactory = clientFactory;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var client = _clientFactory.CreateClient();
        var dummyProducts =
            await client.GetFromJsonAsync<DummyProductsResponse>(
                $"https://dummyjson.com/products?limit={DummyProductCount}");

        var createdUsers = await SeedUsersAsync();
        await SeedProductsAsync(dummyProducts!.Products, createdUsers);
    }

    private async Task<List<UserData>> SeedUsersAsync()
    {
        var users = new List<UserData>();

        users.Add(new UserData(await CreateUserAsync("ArthurDent", "ArthurDent@abp.io", "Arthur", "Dent")));
        users.Add(new UserData(await CreateUserAsync("JohnDoe", "JohnDoe@abp.io", "John", "Doe")));
        users.Add(new UserData(await CreateUserAsync("JaneSmith", "JaneSmith@abp.io", "Jane", "Smith")));
        users.Add(new UserData(await CreateUserAsync("MichaelBrown", "MichaelBrown@abp.io", "Michael", "Brown")));
        users.Add(new UserData(await CreateUserAsync("EmilyClark", "EmilyClark@abp.io", "Emily", "Clark")));
        users.Add(new UserData(await CreateUserAsync("DavidWilson", "DavidWilson@abp.io", "David", "Wilson")));
        users.Add(new UserData(await CreateUserAsync("SarahMiller", "SarahMiller@abp.io", "Sarah", "Miller")));
        users.Add(new UserData(await CreateUserAsync("JamesTaylor", "JamesTaylor@abp.io", "James", "Taylor")));
        users.Add(new UserData(await CreateUserAsync("LauraAnderson", "LauraAnderson@abp.io", "Laura", "Anderson")));
        users.Add(new UserData(await CreateUserAsync("RobertThomas", "RobertThomas@abp.io", "Robert", "Thomas")));
        users.Add(new UserData(await CreateUserAsync("SophiaMartinez", "SophiaMartinez@abp.io", "Sophia", "Martinez")));
        users.Add(new UserData(await CreateUserAsync("DanielGarcia", "DanielGarcia@abp.io", "Daniel", "Garcia")));
        users.Add(new UserData(await CreateUserAsync("OliviaHarris", "OliviaHarris@abp.io", "Olivia", "Harris")));
        users.Add(new UserData(await CreateUserAsync("MatthewLopez", "MatthewLopez@abp.io", "Matthew", "Lopez")));
        users.Add(new UserData(await CreateUserAsync("IsabellaHall", "IsabellaHall@abp.io", "Isabella", "Hall")));
        users.Add(new UserData(await CreateUserAsync("ChristopherYoung", "ChristopherYoung@abp.io", "Christopher","Young")));
        users.Add(new UserData(await CreateUserAsync("GraceKing", "GraceKing@abp.io", "Grace", "King")));
        users.Add(new UserData(await CreateUserAsync("AnthonyWright", "AnthonyWright@abp.io", "Anthony", "Wright")));
        users.Add(new UserData(await CreateUserAsync("HannahScott", "HannahScott@abp.io", "Hannah", "Scott")));
        users.Add(new UserData(await CreateUserAsync("JoshuaGreen", "JoshuaGreen@abp.io", "Joshua", "Green")));
        users.Add(new UserData(await CreateUserAsync("ChloeBaker", "ChloeBaker@abp.io", "Chloe", "Baker")));

        return users;
    }

    private async Task<UserData> CreateUserAsync(string userName, string email, string name, string surname)
    {
        var user = new IdentityUser(_guidGenerator.Create(), userName, email)
        {
            Name = name,
            Surname = surname
        };

        await _identityUserManager.CreateAsync(user, _guidGenerator.Create().ToString().Substring(0, 8),
            validatePassword: false);

        var userDate = new UserData(user.Id, user.UserName, user.Email, user.Name, user.Surname);
        await _cmsUserRepository.InsertAsync(new CmsUser(userDate));

        return userDate;
    }

    private async Task SeedProductsAsync(List<DummyProduct> products, List<UserData> users)
    {
        foreach (var dummyProduct in products)
        {
            var categoryId = await GetCategoryIdAsync(dummyProduct.Category.Split("-").First().ToPascalCase());

            if (!await _productRepository.AnyAsync(x => x.Name == dummyProduct.Title))
            {
                var image = await DownloadImageAsBytesAsync(dummyProduct.Images?.FirstOrDefault() ?? string.Empty);
                var product = await _productManager.CreateAsync(
                    categoryId,
                    dummyProduct.Title,
                    dummyProduct.Price,
                    dummyProduct.Stock,
                    image,
                    dummyProduct.Description
                );

                if (dummyProduct.Reviews != null)
                {
                    if (dummyProduct.Reviews.Count > users.Count)
                    {
                        dummyProduct.Reviews = dummyProduct.Reviews.Take(users.Count).ToList();
                    }

                    var random = new Random();
                    var randomUsers = users.OrderBy(x => random.Next()).Take(dummyProduct.Reviews.Count).ToList();

                    for (var i = 0; i < dummyProduct.Reviews.Count; i++)
                    {
                        var review = dummyProduct.Reviews[i];
                        var cmsUser = new CmsUser(randomUsers[i]);
                        var comment = await _commentManager.CreateAsync(cmsUser, "Product",product.Id.ToString(),review.Comment);
                        await _commentRepository.InsertAsync(comment);
                    }
                }
            }
        }
    }

    private async Task<Guid> GetCategoryIdAsync(string categoryName)
    {
        var categoryId = (await _categoryRepository.FirstOrDefaultAsync(x => x.Name == categoryName))?.Id;

        if (categoryId == null)
        {
            var category = await _categoryManager.CreateAsync(categoryName);

            await _unitOfWorkAccessor.UnitOfWork.SaveChangesAsync();

            return category.Id;
        }

        return categoryId.Value;
    }

    public async Task<byte[]> DownloadImageAsBytesAsync(string url)
    {
        if (url.IsNullOrWhiteSpace())
        {
            return [];
        }

        using var http = new HttpClient();
        await using var stream = await http.GetStreamAsync(url);
        using var image = await Image.LoadAsync(stream);
        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return ms.ToArray();
    }

    private class DummyProductsResponse
    {
        public List<DummyProduct> Products { get; set; }
    }

    private class DummyProduct
    {
        public string Title { get; set; } = "";

        public double Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = "";

        public string Category { get; set; } = "";

        public List<string>? Images { get; set; }

        public List<DummyProductReview>? Reviews { get; set; }
    }

    private class DummyProductReview
    {
        public string Comment { get; set; } = "";
    }
}