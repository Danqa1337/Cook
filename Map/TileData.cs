using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class TileData : IEquatable<TileData>
{
    public bool IsMaped { get; private set; }
    public readonly int Index;

    public TileData(int index)
    {
        IsMaped = false;
        Index = index;
    }

    public static TileData Null
    {
        get
        {
            return new TileData(-1);
        }
    }

    public int2 Position => Index.ToMapPosition();
    public int x => Position.x;
    public int y => Position.y;

    public bool isValid
    {
        get
        {
            if (this == Null || Position.x < 0 || Position.y < 0 || Position.x > 63 || Position.y > 63)
            {
                return false;
            }
            return true;
        }
    }

    public bool isBorderTile()
    {
        if (Position.x == 0 || Position.y == 0 || Position.x == 63 || Position.y == 63)
        {
            return true;
        }
        return false;
    }

    public int GetDistanceFromEdge()
    {
        return Mathf.Min(x, 64 - x, y, 64 - y);
    }

    public static int2 operator +(TileData A, TileData B)
    {
        return A.Position + B.Position;
    }

    public static int2 operator -(TileData A, TileData B)
    {
        return A.Position - B.Position;
    }

    public static bool operator ==(TileData A, TileData B)
    {
        return A.Index == B.Index;
    }

    public static bool operator !=(TileData A, TileData B)
    {
        return A.Index != B.Index;
    }

    public override string ToString()
    {
        if (this == Null) return "Tile Null";
        else return String.Format("Tile {0}, {1}", Position.x, Position.y);
    }

    public bool Equals(TileData other)
    {
        return Index == other.Index;
    }

    public void MarkAsMaped()
    {
        IsMaped = true;
    }

    public void MarkAsUnmaped()
    {
        IsMaped = false;
    }
}

public class NullTileException : Exception
{
    public override string ToString()
    {
        return "Null tile exception: You are trying to access Null tile data. It is not allowed";
    }
}