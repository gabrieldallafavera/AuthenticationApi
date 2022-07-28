namespace Api
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
