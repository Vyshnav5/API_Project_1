using API_Project_1.Data;
using API_Project_1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Project_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _context.Product.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            return await _context.Product.ToListAsync();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Brand>> GetBrands(int Id)
        {
            if (_context.Product == null)
            {
                return NotFound();

            }
            var Product1 = await _context.Product.FindAsync(Id);

            if (Product1 == null)
            {
                return NotFound();
            }

            return Product1;

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return BadRequest();
            }
            _context.Product.Update(brand);
            //_dbContext.Entry (brand). State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Brand>> DeleteBrand(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }

            var brand = await _context.Product.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }
            _context.Product.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile([FromForm] FileUpload filesmodel)
        {
            ModelState.Remove("Id");
            if (filesmodel.File == null && filesmodel.File.Length == 0)
            {

            }
            var folderName = Path.Combine("Resources", "All files");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            var filename = filesmodel.File.FileName;
            var fullpath = Path.Combine(pathToSave, filename);

            var dbpath = Path.Combine(folderName, filename);
            filesmodel.filepath = fullpath;
            filesmodel.Id = 0;

            if (System.IO.File.Exists(fullpath))
            {
                return BadRequest("file already exist");
            }

            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                _context.Upload.Add(filesmodel);
                await _context.SaveChangesAsync();

                filesmodel.File.CopyTo(stream);

            }
            return Ok(new { dbpath });
        }

        [HttpPost]
        [Route("Reg")]
        public async Task<ActionResult<Registration>> Register(Registration Reg)
        {
            _context.Regster.Add(Reg);
            await _context.SaveChangesAsync();
            return Reg;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(Login log)
        {
            var tempUser = _context.Regster.FirstOrDefault(u => u.Email == log.Email && u.Password == log.Password);
            if (tempUser != null)
            {
                var token = CreateToken();
                return Ok(new { status = true, Message = "success", Data = new { Token = token } });
                //return Ok();
            }
            return BadRequest();
        }

        private string CreateToken()
        {
            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Sub,"1"),
                new Claim(JwtRegisteredClaimNames.Email,"gana"),

                   new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this  is my custom secret key for authentication"));
            var token = new JwtSecurityToken(
                issuer: "https://example.com",
                audience: "https://example.com",
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        
        }
    }
}

    

