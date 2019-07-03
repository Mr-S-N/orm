using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Query
    {
        public List<string> querys = new List<string>();
        private List<string> tabel = new List<string>();

        private string table;
        private int  st = 0;
        public Query (string table)
        {
            this.table = table;
        }

        public Query FindAll()
        {
            querys.Add($"select * from {this.table}");
            return this;
        }
        public Query FindByFilds(params string[] arg)
        {
            string temp = "";

            for (int i = 0; i < arg.Length - 1; i++)
            {
                temp += arg[i] + ",";
            }
            temp += arg[arg.Length - 1];

            querys.Add($"select {temp} from {this.table}");

            return this;
        }
        public Query Field(string field)
        {
            if (this.st == 0)
            {
                this.querys.Add("where " + field + " ");
            }
            else
            {
                this.querys.Add(" and " + field + " ");
            }
          




            return this;
        }

        private void Combine(string s)
        {
            int c = querys.Count;
            querys[c - 1] += s;
            
        }
        public Query Equals(int  value)
        {
            Combine($"= {value}");
  

            this.st++;

            return this;
        }
        public Query Equals(string value)
        {
            Combine($"= '{value}' ");
      

            this.st++;

            return this;
        }

        public Query LessThen(int value)
        {
            Combine($"< {value} ");
        

         

            this.st++;

            return this;
        }

        public Query GreaterOrEquals(int value)
        {
            Combine($">= {value} ");
         
            

            this.st++;

            return this;
        }

        public Query SortBy(string valuetoSort)
        {
            this.querys.Add($"order by {valuetoSort} ");


            return this;
        }

        public Query Join (params Query[] query)
        {
            
            int c = query.Length;
            querys.Add( "select");
            string temp = "";
            for (int i = 0; i < c; i++)
            {
                temp += $"t{i}.*,";
                tabel.Add($"t{i}");
            }
            temp = temp.Remove(temp.Length - 1);
            querys.Add(temp);
            querys.Add("from(");

            for (int i = 0; i < query.Length; i++)
            {
                if((i>0) )
                {
                    this.querys.Add("(");
                }
                this.querys.AddRange(query[i].querys);
                this.querys.Add($") as t{i} ");
                if(i!=query.Length-1)
                {
                    this.querys.Add(" inner join");
                }
            }
          
            //this.querys.Add(" join ");

            //   this.querys.Add(query.queryName + " ");

            return this;
        }

        public Query On(string f,string i)
        {
            this.querys.Add($"on {tabel[0]}.{f} = {tabel[1]}.{i}");
      
            return this;
        }

        public override string ToString()
        {
            string full = null;
            foreach (var item in querys)
            {
                full += item + "\n";
            }
            return full;
        }



    }
}
