namespace CellReport_Workflow.Models.User
{
    public interface IUser
    {
        public User getUserInfo(User cUser);
        public Boolean getAllUser(User cUser);
    }
}
