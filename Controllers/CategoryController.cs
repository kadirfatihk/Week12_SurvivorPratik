using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week12_SurvivorPratik.Context;
using Week12_SurvivorPratik.Entities;

namespace Week12_SurvivorPratik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Veritabanı işlemleri için kullanılacak DbContext örneği
        private readonly SurvivorDbContext _context;

        // Constructor metodu, kontrolcü oluşturulurken DbContext örneğini alır.
        public CategoryController(SurvivorDbContext context)
        {
            _context = context;
        }

        // Tüm kategorileri listelemek için kullanılan Get endpointi.
        [HttpGet]
        public IActionResult GetAll()
        {
            // _context.Categories -> DbSet'inden tüm kategorileri getirir.
            // Include metodu -> her kategorinin "Compotitors" ilişkisini yükler.
            var categories = _context.Categories.Include(x=>x.Competitors).ToList();

            // Kategorileri içeren bir liste döndürür.
            // Ok() metodu -> 200 OK HTTP durum kodunu ayarlar ve verileri döndürür.
            return Ok(categories);
        }

        // Belirli bir ID'ye sahip kategoriyi getirmek için kullanılan GET endpoint'i.
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories.FirstOrDefault(x=>x.Id == id);

            // Kategori bulunamazsa 404 Not Found döner.
            if (category is null)
                return NotFound();

            // Bulunan kategoriyi 200 OK ile döndürür.
            return Ok(category);
        }

        // Yeni bir kategori eklemek için kullanılan POST endpoint'i.
        [HttpPost]
        public IActionResult Add([FromBody] CategoryEntity category)
        {
            if (category is null)
                return BadRequest();

            _context.Categories.Add(category);      // Yeni kategoriyi veritabanına ekler.
            _context.SaveChanges();     // Yeni kategoriyi veritabanına kaydeder.

            // 201 Created durum kodunu ve oluşturulan kategorinin konumunu içeren bir başlık döndürür.
            return CreatedAtAction(nameof(GetById), new {id = category.Id}, category);

        }

        // Mevcut bir kategoriyi güncellemek için kullanılan PUT endpoint'i.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryEntity category)
        {
            // İstek gövdesi boşşa veya ID uyuşmuyorsa 400 BadRequest döner.
            if (category is null || id != category.Id)
                return BadRequest();

            // Güncellenecek kategoriyi veritabanında arar.
            var existingCategory = _context.Categories.FirstOrDefault(x=>x.Id == id);

            // Kategori bulunmazsa 404 NotFound() döner.
             if (existingCategory is null)
                return NotFound();

            existingCategory.Name = category.Name;      // Kategori adını günceller.

            _context.SaveChanges();     // Değişiklikleri veritabanına kaydeder.

            return NoContent();     // 204 NoContent() döner. Güncelleme başarılı ancak geri dönülecek veri yok.
        }

        // Belirli bir ID'ye ait kategoriyi silmek için kullanılan DELETE endpoint'i.
        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            // Silinecek kategoriyi veritabanında arar.
            var category = _context.Categories.FirstOrDefault(x=>x.Id==id);

            if (category is null)
                return NotFound();

            category.IsDeleted = true;      // Soft delete için IsDeleted özelliğini true olarak işaretler.

            _context.SaveChanges();
               
            return NoContent();     // 204 NoContent() ile döner. Silme başarılı ancak geriye döndürülecek veri yok.
        }
    }
}
