using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int ID{get;set;}
        public string Url{get;set;}
        public bool IsMain{get;set;}
        public string PublicId{get;set;}
        public UserApp User{get;set;}
        public int UserId{get;set;}
    }
}