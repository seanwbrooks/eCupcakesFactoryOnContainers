namespace Api.Models{
    public class OrderRequest{
       public int Id { get; set; }
       public string Flavour { get; set; }
       public ItemSize Size { get; set; }
       public int Quantity { get; set; }
    }

    public class MixedOrder : OrderRequest{
        public string MixedBy {get;set;}
        public string MixedOn {get;set;}
    }

    public class BakedOrder: MixedOrder{
        public string BakedBy {get;set;}
        public string BakedOn{get;set;}
    }

    public enum ItemSize{S,M,L,XL}
}