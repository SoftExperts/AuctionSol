// IUserConnectionManager.cs
using System.Collections.Concurrent;
using System.Collections.Generic;

public interface IUserConnectionManager
{

    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId, string connectionId);
    IEnumerable<string> GetConnections(string userId);
    void UpdateUserStatus(string userId, string status);
    public Dictionary<string, string> GetUserStatuses();
}


public class UserConnectionManager : IUserConnectionManager
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _userConnections = new ConcurrentDictionary<string, HashSet<string>>();
    public Dictionary<string, string> _userStatuses = new Dictionary<string, string>();

    public void AddConnection(string userId, string connectionId)
    {
        if (_userConnections.ContainsKey(userId) == false)
        {
            var connections = _userConnections.GetOrAdd(userId, new HashSet<string>());
            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

    }

    public void RemoveConnection(string userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            lock (connections)
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }
    }

    public IEnumerable<string> GetConnections(string userId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            return connections;
        }
        return new List<string>();
    }

    public void UpdateUserStatus(string userId, string status)
    {
        lock (_userStatuses)
        {
            _userStatuses[userId] = status;
        }
    }

    public Dictionary<string, string> GetUserStatuses()
    {
        return _userStatuses;
    }
}
