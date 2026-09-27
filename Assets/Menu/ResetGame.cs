public static class GameReset
{
    public static void ResetGame()
    {
        if (GovernmentStabilityManager.Instance != null)
            GovernmentStabilityManager.Instance.SetInstability(0);

        if (MoneyManager.Instance != null)
            MoneyManager.Instance.Set(1000);

        // ‘¼‚à‚±‚±‚É’Ç‰Á
    }
}
