namespace UserManagementAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
