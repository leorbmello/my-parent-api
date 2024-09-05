using System.Collections.Concurrent;

namespace MyParentApi.Application.Managers
{
    public static class TokenBlackListMgr
    {
        private static ConcurrentDictionary<string, string> blackListedTokens = new();

        static TokenBlackListMgr()
        {
            // TODO: nothing
        }

        public static void AddToBlackList(string token)
        {
            blackListedTokens.TryAdd(token, token);
        }

        public static bool IsTokenBlackListed(string token)
        {
            return blackListedTokens.ContainsKey(token);
        }
    }
}
