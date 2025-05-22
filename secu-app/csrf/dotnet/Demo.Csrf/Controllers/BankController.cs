using Demo.Csrf.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Csrf.Controllers
{
    public class BankController : Controller
    {
        [HttpGet]
        public IActionResult FakeTransfer()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Transfer([FromQuery] int AccountNo, decimal Amount)
        {
            return View("CreateTransfer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(BankTransferRequestPayload payload)
        {
            Console.WriteLine($"Transfering {payload.Amount} to account {payload.AccountNo}");

            return View("TransferDone");
        }
    }
}
