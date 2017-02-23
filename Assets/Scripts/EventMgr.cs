using System;
using System.Collections.Generic;


delegate void EventCallback1();
delegate void EventCallback2(EventArg arg);

enum EventID
{
    SpriteJump = 0,
}
class EventArg
{
    private object[] m_args;
    public EventArg(params object[] args)
    {
        this.m_args = args;
    }

    public object this[int index] { get { return this.Check(index) ? this.m_args[index] : null; } }

    public object[] Args { get { return this.m_args; } }

    //检查参数是否正确
    private bool Check(int index)
    {
        return this.m_args != null && index >= 0 && index < this.m_args.Length;
    }
}

static class EventMgr
{
    class EventCBDelegate
    {
        private EventID m_id;
        private List<Delegate> m_cbs;
        public EventCBDelegate(EventID id)
        {
            this.m_id = id;
            this.m_cbs = new List<Delegate>();
        }

        //注册消息的个数
        public int Count { get { return this.m_cbs.Count; } }

        //新加一个消息接收回调
        public void Add(Delegate dlg)
        {
            this.m_cbs.Add(dlg);
        }

        //移除一个消息回调
        public void Remove(Delegate dlg)
        {
            this.m_cbs.Remove(dlg);
        }

        //回调消息
        public void Call(EventArg arg)
        {
            for (int i = 0; i < this.m_cbs.Count; i++)
            {
                Delegate dlg = this.m_cbs[i];
                if (dlg.GetType() == typeof(EventCallback1))
                    ((EventCallback1)dlg)();
                else if (dlg.GetType() == typeof(EventCallback2))
                    ((EventCallback2)dlg)(arg);
            }
        }
    }

    private static Dictionary<EventID, EventCBDelegate> sm_events;              //回调事件队列

    static EventMgr()
    {
        sm_events = new Dictionary<EventID, EventCBDelegate>();
    }

    //注册与注销处理
    private static void Resister(EventID id, Delegate cb)
    {
        EventCBDelegate dlg;
        if (!sm_events.TryGetValue(id, out dlg))
        {
            dlg = new EventCBDelegate(id);
            sm_events.Add(id, dlg);
        }
        dlg.Add(cb);
    }

    private static void UnResister(EventID id, Delegate cb)
    {
        if (!sm_events.ContainsKey(id)) return;
        sm_events[id].Remove(cb);
        if (sm_events[id].Count <= 0)
            sm_events.Remove(id);
    }

    //---------------------------------------
    //public
    //---------------------------------------

    //用户注册，注销接口
    public static void Resister(EventID id, EventCallback1 cb)
    {
        Resister(id, (Delegate)cb);
    }

    public static void UnResister(EventID id, EventCallback1 cb)
    {
        UnResister(id, (Delegate)cb);
    }

    public static void Resister(EventID id, EventCallback2 cb)
    {
        Resister(id, (Delegate)cb);
    }

    public static void UnResister(EventID id, EventCallback2 cb)
    {
        UnResister(id, (Delegate)cb);
    }

    //用户回调消息接口
    public static void Call(EventID id)
    {
        Call(id, new EventArg(null));
    }

    public static void Call(EventID id, EventArg arg)
    {
        EventCBDelegate dlg;
        if (sm_events.TryGetValue(id, out dlg))
        {
            dlg.Call(arg);
        }
    }
}