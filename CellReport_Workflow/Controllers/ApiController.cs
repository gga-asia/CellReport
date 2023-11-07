using CellReport_Workflow.Interface;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CellReport_Workflow.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly IEncryptService _IEncryptService;
        public ApiController(IEncryptService IEncryptService)
        {
            _IEncryptService = IEncryptService;
        }

        [HttpPost]
        public IActionResult getToken([FromBody] VMString vMString)
        {
            string Para = "";
            string Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            Time = Time.Replace("/", "_").Replace(" ", "-");
            if (!string.IsNullOrEmpty(vMString.Emp_Id))
            {
                Para = Time + "*" + vMString.Emp_Id;
                Para = _IEncryptService.Encrypt(Para);
                return Ok(Para);
            }
            else
                return BadRequest();
        }
    }
}
