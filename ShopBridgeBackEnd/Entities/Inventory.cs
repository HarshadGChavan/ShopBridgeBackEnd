using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBridgeBackEnd.Entities
{

    [Table("Inventories")]
    public class Inventory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
