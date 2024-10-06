using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week12_SurvivorPratik.Context;
using Week12_SurvivorPratik.Entities;

namespace Week12_SurvivorPratik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorController(SurvivorDbContext context)
        {
            _context = context;
        }

        // Tüm yarışmacıları listelemek için kullanılan GET endpoint'i.
        [HttpGet]
        public IActionResult GetAll()
        {
            // _Context.Competitors -> DbSet'inden tüm yarışmacıları getirir.
            // Include() metodu -> Her yarışmacının "Category" ilişkisini yükler.
            var competitors = _context.Competitors.Include(x => x.Category).ToList();

            return Ok(competitors);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Belirtilen ID'ye ait yarışmacıyı arar.
            var competitors = _context.Competitors.FirstOrDefault(x => x.Id == id);

            if (competitors is null)
                return NotFound();

            return Ok(competitors);
        }

        [HttpGet("ByCategoryId/{categoryId}")]
        public IActionResult GetCategoryId(int categoryId)
        {
            // Belirlenen kategori ID'sine sahip olan yarışmacıları arar.
            var competitors = _context.Competitors.Where(x => x.CategoryId == categoryId).ToList();

            // Liste boş mu kontrolü yapılır.
            if (competitors.Count is 0) 
                return NotFound();

            return Ok(competitors);
        }

        [HttpPost]
        public IActionResult Add([FromBody] CompetitorEntity competitor)
        {
            // İstek gövdesi boşşsa 400 BadRequest() döner.
            if (competitor is null)
                return BadRequest();

            _context.Competitors.Add(competitor);

            _context.SaveChanges();

            // 201 Created durum kodunu oluşturur ve oluşturulan yarışmacının konumunu içeren bir başlık döner.
            return CreatedAtAction(nameof(GetById), new { id = competitor.Id }, competitor);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CompetitorEntity competitor)
        {
            if (competitor is null || id != competitor.Id)
                return BadRequest();

            var existingCompetitor = _context.Competitors.FirstOrDefault(x => x.Id == id);

            if (existingCompetitor is null)
                return NotFound();

            // Yarışmacı bilgilerini günceller.
            existingCompetitor.FirstName = competitor.FirstName;
            existingCompetitor.LastName = competitor.LastName;
            existingCompetitor.CategoryId = competitor.CategoryId;
            existingCompetitor.ModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            var competitor = _context.Competitors.FirstOrDefault(x=>x.Id==id);

            if (competitor is null)
                return NotFound();

            competitor.IsDeleted = true;

            _context.SaveChanges();

            return NoContent();
        }
    }
}
