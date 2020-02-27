using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
/*
[System.Serializable]
public class Cell
{
    public Renderer renderer = null;
    public int state = 0;
}*/
public class CellularAutomaton1D : MonoBehaviour
{ 
    
    public Dictionary<int, int[]> RulesetDataBase = new Dictionary<int, int[]>();
    int generation;  // How many generations?
  public int Ruleset;
  int[,] matrix;  // Store a history of generations in 2D array, not just one

  public int cols;
  public int rows;

  private Renderer[,] AllCellsObjects; 
  
  private void Start()
  {
      matrix = new int[cols,rows];
      AllCellsObjects = new Renderer[cols,rows];
      for (int i = 0; i < 255; i++)
      {
          var integerOfRuleset = i;

          var ruleset = new int[8]; // {0, 1, 1, 0, 1, 1, 1, 0}; //30
          for (var j = 0; integerOfRuleset > 0; j++)
          {
              ruleset[j] = integerOfRuleset % 2;
              integerOfRuleset /= 2;
          }
            
          RulesetDataBase.Add(i , ruleset );
      }
      
      for (int i = 0; i < cols; i++)
      {
          for (int j = 0; j < rows; j++)
          {
              var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
              temp.transform.position = new Vector3(i - cols / 2, -j, 0);
              temp.transform.parent = transform;
              var r = temp.GetComponent<Renderer>();
              AllCellsObjects[i, j] = r;
          }
      }

      restart();
  }

  private void Update()
  {
      if (Input.GetKey(KeyCode.Space))
      {
          generate();
          display();
      }
      if (Input.GetKeyDown(KeyCode.Escape))
      {
          restart();
      }
  }

  // Reset to generation 0
  void restart() {
    for (int i = 0; i < cols; i++) {
      for (int j = 0; j < rows; j++) {
        matrix[i,j] = 0;
      }
    }
    matrix[cols/2,0] = 1;    // We arbitrarily start with just the middle cell having a state of "1"
    generation = 0;
  }


  // The process of creating the new generation
  void generate() {

    // For every spot, determine new state by examing current state, and neighbor states
    // Ignore edges that only have one neighor
    for (int i = 0; i < cols; i++) {
      int left  = matrix[(i+cols-1)%cols,generation%rows];   // Left neighbor state
      int me    = matrix[i,generation%rows];       // Current state
      int right = matrix[(i+1)%cols,generation%rows];  // Right neighbor state
      matrix[i,(generation+1)%rows] = rules(left, me, right); // Compute next generation state based on ruleset
    }
    generation++;
  }

  // This is the easy part, just draw the cells, fill 255 for '1', fill 0 for '0'
  void display() {
    int offset = generation%rows;

    for (int i = 0; i < cols; i++) {
      for (int j = 0; j < rows; j++) {
        int y = j - offset;
        if (y <= 0) y = rows + y;
        if (matrix[i,j] == 1) AllCellsObjects[i,j].material.color = Color.white;
        else                  AllCellsObjects[i,j].material.color = Color.black;
      }
    }
  }
  // Implementing the Wolfram rules
  // This is the concise conversion to binary way
  /*int rules (int a, int b, int c) {
   String s = "" + a + b + c;
   int index = Integer.parseInt(s, 2);
   return ruleset[index];
   }*/
  // For JavaScript Mode
  int rules (int a, int b, int c) {
      var ruleset = new int[8]; // {0, 1, 1, 0, 1, 1, 1, 0}; //30
      RulesetDataBase.TryGetValue(Ruleset,out ruleset);
    if (a == 1 && b == 1 && c == 1) return ruleset[7];
    if (a == 1 && b == 1 && c == 0) return ruleset[6];
    if (a == 1 && b == 0 && c == 1) return ruleset[5];
    if (a == 1 && b == 0 && c == 0) return ruleset[4];
    if (a == 0 && b == 1 && c == 1) return ruleset[3];
    if (a == 0 && b == 1 && c == 0) return ruleset[2];
    if (a == 0 && b == 0 && c == 1) return ruleset[1];
    if (a == 0 && b == 0 && c == 0) return ruleset[0];
    return 0;
  }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    public Dictionary<int, int[]> RulesetDataBase = new Dictionary<int, int[]>();

    public int Col = 20;
    public int Row = 50;
    
    private int generation;

    public int RuleSet = 30;

    private int[,] AllCells; 
    private Renderer[,] AllCellsObjects; 

    private void Start()
    {
        AllCells = new int[Col ,Row];
        AllCellsObjects = new Renderer[Col,Row];
       // AllCells[Col / 2,0].state = 1;
        
        for (int i = 0; i < 255; i++)
        {
            var integerOfRuleset = i;

            var ruleset = new int[8]; // {0, 1, 1, 0, 1, 1, 1, 0}; //30
            for (var j = 0; integerOfRuleset > 0; j++)
            {
                ruleset[j] = integerOfRuleset % 2;
                integerOfRuleset /= 2;
            }
            
            RulesetDataBase.Add(i , ruleset );
        }
        
        
        for (int i = 0; i < Col; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = new Vector3(i - Col / 2, j, 0);
                temp.transform.parent = transform;
                var r = temp.GetComponent<Renderer>();
                AllCellsObjects[i, j] = r;
            }
        }
        
        for (var i = 0; i < Col; i++)
        {
            AllCellsObjects[i,generation%Row].material.color = AllCells[i,generation%Row]== 0 ? Color.black : Color.white;
        }
        generation++;
      
        InvokeRepeating(nameof(GenerateNextCA), 0, 0.05f);
    }
    
    private void GenerateNextCA()
    {
        // For every spot, determine new state by examing current state, and neighbor states
        // Ignore edges that only have one neighor
        for (var i = 1; i < Col-1; i++)
        {
            var left = AllCells[i-1,generation%Row]; // Left neighbor state
            var me = AllCells[i,generation%Row]; // Current state
            var right = AllCells[i+1,generation%Row]; // Right neighbor state
            AllCells[i,generation%Row] = rules(left, me, right);
            AllCellsObjects[i,generation%Row].material.color = AllCells[i,generation%Row]== 0 ? Color.black : Color.white;
        }
        generation++;
    }
    private int rules(int a, int b, int c)
    {
        var ruleset = new int[8]; // {0, 1, 1, 0, 1, 1, 1, 0}; //30
        RulesetDataBase.TryGetValue(RuleSet,out ruleset);

        if (a == 1 && b == 1 && c == 1) return ruleset[7];
        if (a == 1 && b == 1 && c == 0) return ruleset[6];
        if (a == 1 && b == 0 && c == 1) return ruleset[5];
        if (a == 1 && b == 0 && c == 0) return ruleset[4];
        if (a == 0 && b == 1 && c == 1) return ruleset[3];
        if (a == 0 && b == 1 && c == 0) return ruleset[2];
        if (a == 0 && b == 0 && c == 1) return ruleset[1];
        if (a == 0 && b == 0 && c == 0) return ruleset[0];
        return 0;
    }*/
}