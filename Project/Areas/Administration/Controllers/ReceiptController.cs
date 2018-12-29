using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System;
using System.Linq;
using X.PagedList;

namespace Project.Areas.Administration.Controllers
{
    public class ReceiptController : BaseController
    {
        private readonly IReceiptService receiptService;
        private readonly IMapper mapper;

        public ReceiptController(IReceiptService receiptService,
            IMapper mapper) {
            this.receiptService = receiptService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/administration/[controller]/all-receipts/")]
        public IActionResult AllReceipts(int? page) {
            int currentPage = page ?? 1;
            ReceiptViewModel[] allReceipts = this.receiptService.GetAll() != null
                ? this.mapper.ProjectTo<ReceiptViewModel>(this.receiptService.GetAll()).ToArray()
                : new ReceiptViewModel[0];
            IPagedList receiptsToDisplay = allReceipts.ToPagedList(currentPage, IntegerConstants.ItemsPerPageInViews);
            return this.View(receiptsToDisplay);
        }

        [HttpGet]
        [Route("/administration/[controller]/receipt-details/{id}")]
        public IActionResult ReceiptDetails(int id) {
            var receipt = this.receiptService.GetById(id)?.First();
            if (receipt != null) {
                ReceiptDetailsViewModel receiptDetailsViewModel = new ReceiptDetailsViewModel {
                    CustomerName = receipt.User.UserName,
                    NamesOfTechniciansHavingWorkedOnTheRepairTask = receipt.ExpertsHavingWorkedOnRepairTask.Select(x => x.Expert.UserName).ToArray(),
                    ReceiptId = receipt.Id,
                    RepairTaskId = receipt.RepairTask.Id,
                    TotalRevenue = receipt.TotalPrice
            };
                return this.View(receiptDetailsViewModel);
            } else {
                throw new ArgumentNullException(String.Format(StringConstants.TryingToRetrieveAMissingReceiptError, id));
            }
        }
    }
}
