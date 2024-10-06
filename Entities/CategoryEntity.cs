namespace Week12_SurvivorPratik.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }

        // Relational Property
        public List<CompetitorEntity> Competitors { get; set; }    // Bir kategorinin birden çok yarışmacısı olabilir.
    }
}
