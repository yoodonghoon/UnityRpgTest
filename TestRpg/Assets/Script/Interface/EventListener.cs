public enum EVENT_TYPE
{
    HP,
}

public interface IListener
{
    public void OnEvent(EVENT_TYPE type, object Param = null);
}
