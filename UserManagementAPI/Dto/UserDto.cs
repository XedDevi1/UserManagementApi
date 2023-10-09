namespace UserManagementAPI.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
    }
}
