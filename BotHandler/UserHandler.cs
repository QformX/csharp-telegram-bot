namespace BotHandler
{
    internal class UserHandler
    {
        private Dictionary<long, bool> _usersCallback;

        public UserHandler() 
        {
            _usersCallback = new Dictionary<long, bool>();
        }

        public void Callback(long userId)
        {

            _usersCallback[userId] = !_usersCallback[userId];
        }

        public void Add(long userId)
        {

            _usersCallback.Add(userId, false);
        }

        public bool UserExist(long userId)
        {
            return _usersCallback.ContainsKey(userId);
        }

        public bool IsUserCallback(long userId)
        {
            return _usersCallback[userId];
        }
    }
}
