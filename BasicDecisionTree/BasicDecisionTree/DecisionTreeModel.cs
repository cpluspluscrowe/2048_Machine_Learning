// Decompiled with JetBrains decompiler
// Type: BasicDecisionTree.DecisionTreeModel
// Assembly: BasicDecisionTree, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F9474D47-4EBB-458D-BC81-4CD6A14EC457
// Assembly location: C:\Users\CCrowe\Documents\2048\BasicDecisionTree\BasicDecisionTree\bin\Debug\BasicDecisionTree.dll

using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
using GameBoard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using TrackTiles;

namespace BasicDecisionTree
{
  public class DecisionTreeModel
  {
    protected DataTable TrainingData;
    protected DecisionTree Model;

    public void TrainModel()
    {
      int[] numArray = new int[11]
      {
        2,
        4,
        8,
        16,
        32,
        64,
        128,
        256,
        512,
        1024,
        2048
      };
      List<TileRows> tileDatabaseCounts = new List<TileRows>();
      foreach (int num in numArray)
      {
        double numberOfRows = DecisionTreeModel.GetNumberOfRows(num);
        Console.WriteLine(numberOfRows.ToString());
        tileDatabaseCounts.Add(new TileRows(numberOfRows, num));
      }
      this.TrainingData = DecisionTreeModel.GetTrainingData(tileDatabaseCounts);
      this.Model = DecisionTreeModel.TrainModel(this.TrainingData);
    }

    public DataTable GetTrainingData()
    {
      return this.TrainingData;
    }

    protected DataTable CreateDataTable(Board board)
    {
      List<int> boardConfiguration = new TrainSwipeData(board).GetConfiguration().GetBoardConfiguration();
      return new DataTable()
      {
        Columns = {
          "v0",
          "v1",
          "v2",
          "v3",
          "v4",
          "v5",
          "v6",
          "v7",
          "v8",
          "v9",
          "v10",
          "v11",
          "v12",
          "v13",
          "v14",
          "v15"
        },
        Rows = {
          {
            (object) boardConfiguration.ElementAt<int>(0),
            (object) boardConfiguration.ElementAt<int>(1),
            (object) boardConfiguration.ElementAt<int>(2),
            (object) boardConfiguration.ElementAt<int>(3),
            (object) boardConfiguration.ElementAt<int>(4),
            (object) boardConfiguration.ElementAt<int>(5),
            (object) boardConfiguration.ElementAt<int>(6),
            (object) boardConfiguration.ElementAt<int>(7),
            (object) boardConfiguration.ElementAt<int>(8),
            (object) boardConfiguration.ElementAt<int>(9),
            (object) boardConfiguration.ElementAt<int>(10),
            (object) boardConfiguration.ElementAt<int>(11),
            (object) boardConfiguration.ElementAt<int>(12),
            (object) boardConfiguration.ElementAt<int>(13),
            (object) boardConfiguration.ElementAt<int>(14),
            (object) boardConfiguration.ElementAt<int>(15)
          }
        }
      };
    }

    public string Predict(Board board)
    {
      DataTable dataTable = this.CreateDataTable(board);
      Codification codification = new Codification(dataTable);
      int[] codewords = this.Model.Decide(codification.Apply(dataTable).ToArray<int>("v0", "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15"));
      return codification.Revert(codewords)[0];
    }

    private static DecisionTree TrainModel(DataTable dt)
    {
      DecisionTree tree = new DecisionTree((IList<DecisionVariable>) new DecisionVariable[16]
      {
        new DecisionVariable("v0", 2048),
        new DecisionVariable("v1", 2048),
        new DecisionVariable("v2", 2048),
        new DecisionVariable("v3", 2048),
        new DecisionVariable("v4", 2048),
        new DecisionVariable("v5", 2048),
        new DecisionVariable("v6", 2048),
        new DecisionVariable("v7", 2048),
        new DecisionVariable("v8", 2048),
        new DecisionVariable("v9", 2048),
        new DecisionVariable("v10", 2048),
        new DecisionVariable("v11", 2048),
        new DecisionVariable("v12", 2048),
        new DecisionVariable("v13", 2048),
        new DecisionVariable("v14", 2048),
        new DecisionVariable("v15", 2048)
      }, 1000);
      Codification codification = new Codification(dt);
      ID3Learning id3Learning = new ID3Learning(tree);
      DataTable table = codification.Apply(dt);
      int[][] array1 = table.ToArray<int>("v0", "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9", "v10", "v11", "v12", "v13", "v14", "v15");
      int[] array2 = table.ToArray<int>("classification");
      return id3Learning.Learn(array1, array2, (double[]) null);
    }

    private static DataTable GetDataTable()
    {
      DataTable dataTable = new DataTable();
      SQLiteConnection sqLiteConnection = new SQLiteConnection("Data Source=C:\\Users\\CCrowe\\Documents\\2048\\training_data.db;Version=3;");
      sqLiteConnection.Open();
      using (SQLiteCommand command = sqLiteConnection.CreateCommand())
      {
        command.CommandText = string.Format("SELECT * FROM {0} limit 10000", (object) "Training");
        new SQLiteDataAdapter(command).Fill(dataTable);
        sqLiteConnection.Close();
        dataTable.TableName = "Training";
        return dataTable;
      }
    }

    private static double GetNumberOfRows(int maxTileValue)
    {
      double num = 0.0;
      SQLiteConnection connection = new SQLiteConnection("Data Source=C:\\Users\\CCrowe\\Documents\\2048\\training_data.db;Version=3;");
      connection.Open();
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format("SELECT count(*) from training where (v0 <= {0} and v1 <= {0} and v2 <= {0} and v3 <= {0} and v4 <= {0}  and v5 <= {0}  and v6 <= {0}  and v7 <= {0}  and v8 <= {0}  and v9 <= {0}  and v10 <= {0}  and v11 <= {0}  and v12 <= {0}  and v13 <= {0}  and v14 <= {0}  and v15 <= {0}) and (v0 = {0} or v1 = {0} or v2 = {0} or v3 = {0} or v4 = {0} or v5 = {0} or v6 = {0} or v7 = {0} or v8 = {0} or v9 = {0} or v10 = {0} or v11 = {0} or v12 = {0} or v13 = {0} or v14 = {0} or v15 = {0} )", (object) maxTileValue), connection))
      {
        SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
        while (sqLiteDataReader.Read())
        {
          double[] numArray = new double[1];
          num = sqLiteDataReader.GetDouble(0);
        }
        sqLiteDataReader.Close();
      }
      connection.Close();
      return num;
    }

    private static DataTable GetTrainingData(List<TileRows> tileDatabaseCounts)
    {
      DataTable dataTable = new DataTable();
      SQLiteConnection sqLiteConnection = new SQLiteConnection("Data Source=C:\\Users\\CCrowe\\Documents\\2048\\training_data.db;Version=3;");
      sqLiteConnection.Open();
      using (SQLiteCommand command = sqLiteConnection.CreateCommand())
      {
        command.CommandText = string.Format("SELECT * FROM {0} limit 10000", (object) "Training");
        string str = "";
        foreach (TileRows tileDatabaseCount in tileDatabaseCounts)
        {
          if (tileDatabaseCount.RowCount > 0.0)
          {
            int num = (int) (tileDatabaseCount.RowCount / 100.0 * 5.0);
            str += string.Format("Select * from( SELECT * from training where (v0 <= {0} and v1 <= {0} and v2 <= {0} and v3 <= {0} and v4 <= {0}  and v5 <= {0}  and v6 <= {0}  and v7 <= {0}  and v8 <= {0}  and v9 <= {0}  and v10 <= {0}  and v11 <= {0}  and v12 <= {0}  and v13 <= {0}  and v14 <= {0}  and v15 <= {0}) and (v0 = {0} or v1 = {0} or v2 = {0} or v3 = {0} or v4 = {0} or v5 = {0} or v6 = {0} or v7 = {0} or v8 = {0} or v9 = {0} or v10 = {0} or v11 = {0} or v12 = {0} or v13 = {0} or v14 = {0} or v15 = {0})  order by delta desc limit {1})", (object) tileDatabaseCount.TileValue, (object) num);
            str += " Union ";
          }
        }
        str.Substring(0, str.Length - 6);
        new SQLiteDataAdapter(command).Fill(dataTable);
        sqLiteConnection.Close();
        dataTable.TableName = "Training";
        return dataTable;
      }
    }
  }
}
