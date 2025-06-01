namespace TomasosPizzeria.Data.Entities
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;

         public ICollection<Dish> Dishes { get; set; }
    }
}
