namespace FormWork.Models
{
    public class Repository
    {
        private static readonly List<Product> _product = new List<Product>();
        private static readonly List<Category> _categories = new List<Category>();
        static Repository()
        {
            if (!_product.Any()) 
            {
                _categories.Add(new Category { CategoryId = 1, CategoryName = "Telephone" });
                _categories.Add(new Category { CategoryId = 2, CategoryName = "Computer" });

                _product.Add(new Product { ProductId = 1, ProductName = "IPhone 15", Price = 15200, Image = "1.png", IsActive = true, CategoryId = 1 });
                _product.Add(new Product { ProductId = 2, ProductName = "Samsung Galaxy", Price = 10200, Image = "2.png", IsActive = true, CategoryId = 1 });
                _product.Add(new Product { ProductId = 3, ProductName = "Macbook Pro", Price = 85000, Image = "3.png", IsActive = true, CategoryId = 2 });
            }
        }
        public static List<Product> Products
        {
            get
            {
                return _product;
            }
        }
        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }
        public static void EditProduct(Product product)
        {
            var entity = _product.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (entity is not null)
            {
                entity.ProductName = product.ProductName;
                entity.Price = product.Price;
                entity.IsActive = product.IsActive;
                entity.CategoryId = product.CategoryId;
                entity.Image = product.Image;
            }
        }
    
    }
}
