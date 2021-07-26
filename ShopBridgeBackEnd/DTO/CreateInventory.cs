using System;
using System.ComponentModel.DataAnnotations;

namespace ShopBridgeBackEnd.DTO
{
    public class CreateInventory
    {
        [Required (ErrorMessage ="Please Provide Inventory Name.")] 
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Provide Inventory Description.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Provide Inventory Price.") ]
        [DataType (DataType.Currency)]
        public decimal Price { get; set; }
    }
}
