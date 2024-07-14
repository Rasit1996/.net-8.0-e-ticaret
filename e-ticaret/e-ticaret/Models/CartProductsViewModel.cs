namespace e_ticaret.Models
{
	public class CartProductsViewModel
	{
        public int ID { get; set; }
        public int urunID { get; set; }
        public string  Name { get; set; }
        public double Price { get; set; }
        public int Number { get; set; }
        public string  ImageUrl { get; set; }
        public int[]? FeaturesID { get; set; }
        public string[]?  FeaturesName { get; set; }
        public DateTime? Sellby { get; set; }
        public string?  Usurname { get; set; }
        public int? SalesID { get; set; }

    }
}
