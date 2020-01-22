﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Material desert;
    public Material fire;
    public Material forest;
    public Material grass;
    public Material ocean;
    public Material pollutedOcean;
    public Material shrub;
    public Material wood;
    public enum BlockType { Desert, Forest, Grass, Ocean, Shrub, Wood }
    //[HideInInspector]
    public BlockType type = BlockType.Ocean;
    public enum BlockStatus { Normal, Fire, Scorch, Polluted }
    //[HideInInspector]
    public BlockStatus status = BlockStatus.Normal;
    public bool hasAnimals;
    private float fireTimer = 10;
    private float pollutedTimer = 10;
    private float saveAnimalTimer = 10;
    //[HideInInspector]
    public Vector2Int coordinate;
    [HideInInspector]
    public GameManager gm;
    private bool fireExtended = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        switch (type)
        {
            case BlockType.Desert:
                GetComponent<Renderer>().material = desert;
                break;
            case BlockType.Forest:
                GetComponent<Renderer>().material = forest;
                break;
            case BlockType.Grass:
                GetComponent<Renderer>().material = grass;
                break;
            case BlockType.Ocean:
                GetComponent<Renderer>().material = ocean;
                break;
            case BlockType.Shrub:
                GetComponent<Renderer>().material = shrub;
                break;
            case BlockType.Wood:
                GetComponent<Renderer>().material = wood;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == BlockType.Forest || type == BlockType.Grass || type == BlockType.Shrub || type == BlockType.Wood)
        {
            if (status == BlockStatus.Fire)
            {
                //if on fire, display fire material
                GetComponent<Renderer>().materials = new Material[2] { GetComponent<Renderer>().material, fire };
                fireTimer -= Time.deltaTime;
                //nearby ocean block will be polluted immediately
                OceanPollute();
                //after few seconds, fire extends
                if (fireTimer <= 8 && !fireExtended)
                {
                    FireExtend();
                }
                //after few seconds, become desert
                if (fireTimer <= 0)
                {
                    type = BlockType.Desert;
                    status = BlockStatus.Normal;
                    GetComponent<Renderer>().materials = new Material[1] { desert };
                }
            }
        }
        else if(type == BlockType.Ocean)
        {
            switch (status)
            {
                case BlockStatus.Normal:
                    if (GetComponent<Renderer>().material != ocean)
                    {
                        GetComponent<Renderer>().material = ocean;
                    }
                    break;
                case BlockStatus.Polluted:
                    if (GetComponent<Renderer>().material != pollutedOcean)
                    {
                        GetComponent<Renderer>().material = pollutedOcean;
                    }
                    break;
                default:
                    status = BlockStatus.Normal;
                    break;
            }
        }
        else if(type == BlockType.Desert)
        {
            if(status != BlockStatus.Normal)
            {
                status = BlockStatus.Normal;
            }
        }

        if (status == BlockStatus.Scorch)
        {
            //add a scorch material later
            GetComponent<Renderer>().materials = new Material[1] { GetComponent<Renderer>().material };
        }
    }

    private void FireExtend()
    {
        fireExtended = true;
        BlockManager bm;
        //check up, down, left, right 4 blocks
        if (coordinate.x - 1 >= 0)
        {
            bm = gm.grid[coordinate.y][coordinate.x - 1].GetComponent<BlockManager>();
            if (Random.Range(0, 3) > 1 && bm.type != BlockType.Ocean && bm.type != BlockType.Desert)
            {
                bm.status = BlockStatus.Fire;
            }
        }
        if (coordinate.x + 1 < gm.grid[coordinate.y].Count)
        {
            bm = gm.grid[coordinate.y][coordinate.x + 1].GetComponent<BlockManager>();
            if (Random.Range(0, 3) > 1 && bm.type != BlockType.Ocean && bm.type != BlockType.Desert)
            {
                bm.status = BlockStatus.Fire;
            }
        }
        if (coordinate.y - 1 >= 0)
        {
            bm = gm.grid[coordinate.y - 1][coordinate.x].GetComponent<BlockManager>();
            if (Random.Range(0, 3) > 1 && bm.type != BlockType.Ocean && bm.type != BlockType.Desert)
            {
                bm.status = BlockStatus.Fire;
            }
        }
        if (coordinate.y + 1 < gm.grid.Count)
        {
            bm = gm.grid[coordinate.y + 1][coordinate.x].GetComponent<BlockManager>();
            if (Random.Range(0, 3) > 1 && bm.type != BlockType.Ocean && bm.type != BlockType.Desert)
            {
                bm.status = BlockStatus.Fire;
            }
        }
    }
    private void OceanPollute()
    {
        BlockManager bm;
        //check nearby 8 blocks
        if (coordinate.x - 1 >= 0)
        {
            bm = gm.grid[coordinate.y][coordinate.x - 1].GetComponent<BlockManager>();
            if (bm.type == BlockType.Ocean)
            {
                bm.status = BlockStatus.Polluted;
            }
            if (coordinate.y - 1 >= 0)
            {
                bm = gm.grid[coordinate.y - 1][coordinate.x - 1].GetComponent<BlockManager>();
                if (bm.type == BlockType.Ocean)
                {
                    bm.status = BlockStatus.Polluted;
                }
            }
        }
        if (coordinate.x + 1 < gm.grid[coordinate.y].Count)
        {
            bm = gm.grid[coordinate.y][coordinate.x + 1].GetComponent<BlockManager>();
            if (bm.type == BlockType.Ocean)
            {
                bm.status = BlockStatus.Polluted;
            }
            if (coordinate.y + 1 < gm.grid.Count)
            {
                bm = gm.grid[coordinate.y + 1][coordinate.x + 1].GetComponent<BlockManager>();
                if (bm.type == BlockType.Ocean)
                {
                    bm.status = BlockStatus.Polluted;
                }
            }
        }
        if (coordinate.y - 1 >= 0)
        {
            bm = gm.grid[coordinate.y - 1][coordinate.x].GetComponent<BlockManager>();
            if (bm.type == BlockType.Ocean)
            {
                bm.status = BlockStatus.Polluted;
            }
            if (coordinate.x + 1 < gm.grid[coordinate.y].Count)
            {
                bm = gm.grid[coordinate.y - 1][coordinate.x + 1].GetComponent<BlockManager>();
                if (bm.type == BlockType.Ocean)
                {
                    bm.status = BlockStatus.Polluted;
                }
            }
        }
        if (coordinate.y + 1 < gm.grid.Count)
        {
            bm = gm.grid[coordinate.y + 1][coordinate.x].GetComponent<BlockManager>();
            if (bm.type == BlockType.Ocean)
            {
                bm.status = BlockStatus.Polluted;
            }
            if (coordinate.x - 1 >= 0)
            {
                bm = gm.grid[coordinate.y + 1][coordinate.x - 1].GetComponent<BlockManager>();
                if (bm.type == BlockType.Ocean)
                {
                    bm.status = BlockStatus.Polluted;
                }
            }
        }
    }
}