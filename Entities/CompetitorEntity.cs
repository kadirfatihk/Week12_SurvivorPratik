namespace Week12_SurvivorPratik.Entities
{
    public class CompetitorEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CategoryId { get; set; }

        // Relational Property
        public CategoryEntity Category { get; set; }    // Bİr yarışmacı sadece bir kategoride yer alabilir.
    }
}
