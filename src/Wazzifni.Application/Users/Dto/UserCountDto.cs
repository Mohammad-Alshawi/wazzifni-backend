namespace Wazzifni.Users.Dto
{
    public class UserCountDto
    {
        public int AllUsersCount { get; set; }
        public int Admins { get; set; }
        public int Users { get; set; }
        //public int PremiumUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int DeActiveUsers { get; set; }
        //public int UsersLoginThisWeak { get; set; }
        //public int UsersLoginThisMonth { get; set; }
        public int UsersJoinedThisMonth { get; set; }
    }

}
