﻿using System;
using Npgsql;


namespace SEIA
{
	public class DBConnect
	{
		NpgsqlConnection conn = new NpgsqlConnection("Server=db.fe.up.pt;Port=5432;UserId=up201306445;Password=obaptisteegay;Database=up201306445;");
		Int64 age, maxNflaws;
		Int32 nRows;

		/********************************** Basic Functions **********************************/
		// Abre coneção
		public void OpenConn()
		{
			try
			{
				conn.Open();

				//não funciona, recorer a \"SEAI\".\"EXAMPLE_TABLE\"
				/*
                string query = "SET SCHEMA 'SEAI'"; // define o schema do trabalho
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                */

				//Console.WriteLine("Coneção bem sucedida!");
			}
			catch (Exception)
			{
				Console.WriteLine("Erro a abrir coneção!");
			}
		}

		// Termina Coneção
		public void CloseConn()
		{
			try
			{
				conn.Close();
				// Console.WriteLine("Coneção concluída!\n");
			}
			catch (Exception)
			{
				Console.WriteLine("Erro a fechar coneção!");
			}
		}

		// Verifica se um parametro com um determinado id existe 
		public bool VerifyId(int id, string table)
		{
			//this.OpenConn();

			string query = $"SELECT COUNT(*) FROM \"SEAI\".\"{table}\" WHERE \"ID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			Int64 result = (Int64)cmd.ExecuteScalar();
			string result_s = result.ToString();

			if (result == 0)
			{
				//this.CloseConn();
				Console.WriteLine($"Erro: Não existe o ID nº {id} na tabela \"{table}\"!\n");
				return false;
			}
			else
			{
				//this.CloseConn();
				//Console.WriteLine($"Existe o ID nº {id} na tabela \"{table}\"!");
				return true;
			}

		}

		//returnar mmatriz com as falhas que ocorerao numa componente


		/********************************** Query Functions **********************************/
		// Nº de falhas de um equipamento, retorna -1 se não existir o equipamento
		public Int64 NumberFlaws(int id)
		{
			this.OpenConn();
			if (VerifyId(id, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"SELECT COUNT(*) FROM \"SEAI\".\"Flaws\" WHERE \"EquipmentID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			Int64 nFlaws = (Int64)cmd.ExecuteScalar();

			if (nFlaws == 0)
				Console.WriteLine($"O equipamento {id} nunca teve uma falha!");
			else
				Console.WriteLine($"O equipamento {id} teve {nFlaws} falha(s) até à data!");

			this.CloseConn();
			return nFlaws;
		}

		// Idade de um equipamento em meses, retorna -1 se não existir o equipamento
		public Int64 Age(int id)
		{
			this.OpenConn();
			if (VerifyId(id, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"SELECT \"xAge\" FROM \"SEAI\".\"Equipments\" WHERE \"Equipments\".\"ID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
				age = dr.GetInt32(0);

			Console.WriteLine($"O equipamento {id} tem {age} ano(s) em regime de trabalho!");

			this.CloseConn();
			return age;
		}


		// Nº de falhas em todos os dijuntores (!)
		public Int64 TotalFlaws()
		{
			this.OpenConn();
			string query = "SELECT count(\"SEAI\".\"Flaws\".\"ID\"), \"SEAI\".\"Equipments\".\"ID\" " +
				"FROM \"SEAI\".\"Flaws\" GROUP BY equipmentid ORDER BY count(flawID) DESC";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				maxNflaws = dr.GetInt32(0);
				break;
			}

			this.CloseConn();
			return maxNflaws;
		}

		public string[,] SearchFlawsTable(int equipmentID){
			this.OpenConn();

			string query = "SELECT Count(\"SEAI\".\"Flaws\".\"ID\")" +
				"FROM \"SEAI\".\"Flaws\" WHERE \"Flaws\".\"EquipmentID\" = {equipmentID}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();


			while (dr.Read())
			{
				nRows = dr.GetInt32(0);
				break;
			}
			String[,] table = new string[nRows,5];

			query = "SELECT * " +
				"FROM \"SEAI\".\"Flaws\" WHERE \"Flaws\".\"EquipmentID\" = {equipmentID}";
			cmd = new NpgsqlCommand(query, conn);
			dr = cmd.ExecuteReader();


			int i = 0;
			while (dr.Read ()) {
				dr[0].
			}

			this.CloseConn();
			return table;
		}


		/********************************** Edition Functions **********************************/
		//Adicionar/Remover Falha (!)
		public int insertFlaw(int equipmentID, int cause, int seriousness){
			this.OpenConn();
			if (VerifyId(equipmentID, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"INSERT INTO \"SEAI\".\"Flaws\" (\"EquipmentID\", \"CauseID\", \"Seriousness\") VALUES ({equipmentID},{cause},{seriousness}); ";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi inserida a falha com sucesso");
			this.CloseConn();
			return 0;
		}

		public int removeFlaw(int flawID){
			this.OpenConn();
			if (VerifyId(flawID, "Flaws") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"DELETE FROM \"SEAI\".\"Flaws\" WHERE \"Flaws\".\"ID\" = {flawID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi removida a falha com sucesso.");
			this.CloseConn();
			return 0;
		}
			
		//Adicionar/Remover Equipamento (!)
		public void insertEquipment(int type, String name, int year, int maxact, int maxope){
			this.OpenConn();
			string query = $"INSERT INTO \"SEAI\".\"Equipments\" (\"Type\", \"Name\", \"xAge\",\"MaxActivations\",\"MaxOperations\") VALUES ({type},'{name}',{year},{maxact},{maxope});";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi inserido equipamento {name}, do tipo {type}, do ano de {year}, com maxino de ativações {maxact} e operações {maxope}.");
			this.CloseConn();
		}

		public int removeEquipment(int equipmentID){
			this.OpenConn();
			if (VerifyId(equipmentID, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"DELETE FROM \"SEAI\".\"Equipments\" WHERE \"Equipments\".\"ID\" = {equipmentID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi removido o equipamento com o ID {equipmentID}.");
			this.CloseConn();
			return 0;
		}

		//Inserir data da ultima utilizacao
		public int insertLastUseDate(int equipmentID, String dateS){
			this.OpenConn();
			if (VerifyId(equipmentID, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}
			DateTime datef = DateTime.ParseExact(dateS,"dd.M.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

			string query = $"UPDATE \"SEAI\".\"Equipments\" SET \"LastUse\" = '{datef}' WHERE \"SEAI\".\"Equipments\".\"ID\"={equipmentID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi atalizada a data da ultima utilizacao do equipamento com o ID {equipmentID} para {dateS}.");
			this.CloseConn();

			this.CloseConn();
			return 0;
		}

		//Incrementar Nºativações
		public int incrementActivations(int equipmentID){
			int nActivations = 0;
			//chamar a funcao aqui nActivations = SearchActivations(int equipmentID);
			nActivations++;

			this.OpenConn();
			if (VerifyId(equipmentID, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"UPDATE \"SEAI\".\"Equipments\" SET \"NumberActivations\" = '{nActivations}' WHERE \"SEAI\".\"Equipments\".\"ID\"={equipmentID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi incrementado o nºativaçoes para {nActivations} no equipamento: {equipmentID}");
			this.CloseConn();

			return 0;
		}

		//Incrementar Nºoperações
		public int incrementOperations(int equipmentID){
			this.OpenConn();
			if (VerifyId(equipmentID, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			int nOperations = 0;
			//chamar a funcao aqui nOperations = SearchOperations(int equipmentID);
			nOperations++;

			string query = $"UPDATE \"SEAI\".\"Equipments\" SET \"NumberOperations\" = '{nOperations}' WHERE \"SEAI\".\"Equipments\".\"ID\"={equipmentID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi incrementado o nºoperaçoes para {nOperations} no equipamento: {equipmentID}");
			this.CloseConn();

			return 0;
		}

		//Atualizar Equipamento (!)


		/********************************** E N D **********************************/

	}
}

