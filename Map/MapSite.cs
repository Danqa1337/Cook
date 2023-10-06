using UnityEngine;

public class MapSite : MapObjectComponent
{
    [SerializeField] private Sprite _visitedSprite;
    [SerializeField] private Sprite _unVisitedSprite;
    [SerializeField] private MapSiteType _mapSiteType;

    public MapSiteData SiteData
    {
        get
        {
            if (DataHolder.CurrentData.SitesData.Contains(transform.localPosition))
            {
                return DataHolder.CurrentData.SitesData[transform.localPosition.ToVector2()];
            }
            return new MapSiteData(MapSiteType.Misc);
        }
        private set
        {
            if (DataHolder.CurrentData.SitesData.Contains(transform.localPosition))
            {
                DataHolder.CurrentData.SitesData[transform.localPosition.ToVector2()] = value;
            }
        }
    }

    public MapSiteType MapSiteType { get => _mapSiteType; }

    private void OnEnable()
    {
        DataHolder.OnLoaded += OnLoaded;
    }

    private void OnDisable()
    {
        DataHolder.OnLoaded -= OnLoaded;
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        var data = saveData.SitesData[transform.localPosition.ToFloat2()];
        MarkVisited(data.IsVisited);
    }

    protected override void OnPlayerEnter()
    {
        if (!SiteData.IsVisited)
        {
            MarkVisited(true);
        }
    }

    private void MarkVisited(bool value)
    {
        SiteData.MarkVisited(value);
        if (value)
        {
            _mapObject.SpriteRenderer.sprite = _visitedSprite;
        }
        else
        {
            _mapObject.SpriteRenderer.sprite = _unVisitedSprite;
        }
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}

public enum MapSiteType
{
    Misc,
    Town,
}

[System.Serializable]
public class MapSiteData
{
    public bool IsVisited { get; private set; }
    public readonly MapSiteType MapSiteType;

    public MapSiteData(MapSiteType mapSiteType)
    {
        MapSiteType = mapSiteType;
    }

    public void MarkVisited(bool value)
    {
        IsVisited = value;
    }
}