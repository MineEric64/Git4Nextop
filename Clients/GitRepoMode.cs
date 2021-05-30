namespace ProjectGFN.Clients
{
    /// <summary>
    /// 상호작용 해야 할 모드
    /// </summary>
    public enum GitRepoMode
    {
        None,
        Clone,
        Commit,
        Push,
        Pull,
        Fetch
    }
}
