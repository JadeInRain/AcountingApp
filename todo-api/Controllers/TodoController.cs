using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Globalization;



namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly TodoContext todoDb;

        //构造函数把TodoContext 作为参数，Asp.net core 框架可以自动注入TodoContext对象
        public TodoController(TodoContext context)
        {
            this.todoDb = context;
        }


        // GET: api/todo/{id}  id为路径参数
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            var todoItem = todoDb.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        // GET: api/todo
        [HttpGet]
        public ActionResult<List<TodoItem>> GetTodoItems(int? password, string? nickname)
        {
            var query = buildQuery(password, nickname);
            return query.ToList();
        }

        // GET: api/todo/pageQuery?skip=5&&take=10  
        // GET: api/todo/pageQuery?name=课程&&isComplete=true&&skip=5&&take=10
        [HttpGet("pageQuery")]
        public ActionResult<List<TodoItem>> queryTodoItem(int? password, string? nickname, int skip, int take)
        {
            var query = buildQuery(password, nickname).Skip(skip).Take(take);
            return query.ToList();
        }

        private IQueryable<TodoItem> buildQuery(int? password, string? nickname)
        {
            IQueryable<TodoItem> query = todoDb.TodoItems;
            if (password != null)
            {
                query = query.Where(t => t.Password == password);
            }
            if (nickname != null)
            {
                query = query.Where(t => t.Nickname == nickname);
            }
            return query;
        }

        public class StatisticDataQuery
        {
            public int StatisticType { get; set; } // 将Type属性重命名为StatisticType
            public string QueryType { get; set; }
            public int Year { get; set; }
        }



        // POST: api/todo
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem todo)
        {
            try
            {
                todoDb.TodoItems.Add(todo);
                todoDb.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return todo;
        }

        // PUT: api/todo/{id}
        [HttpPut("{id}")]
        public ActionResult<TodoItem> PutTodoItem(int id, TodoItem todo)
        {
            if (id != todo.Id)
            {
                return BadRequest("Id cannot be modified!");
            }
            try
            {
                todoDb.Entry(todo).State = EntityState.Modified;
                todoDb.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTodoItem(int id)
        {
            try
            {
                var todo = todoDb.TodoItems.FirstOrDefault(t => t.Id == id);
                if (todo != null)
                {
                    todoDb.Remove(todo);
                    todoDb.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] TodoItem user)
        {
            if (ModelState.IsValid)
            {
                var todo = todoDb.TodoItems.FirstOrDefault(t => t.Id == user.Id && t.Password == user.Password);
                if (todo != null)
                {
                    Console.WriteLine("1");
                    // 登录验证通过
                    // 返回用户信息等其他数据
                    return Ok(new { statusCode = 200, user = todo });
                }
                else
                {
                    Console.WriteLine("2");
                    // 登录验证失败，返回错误信息
                    return BadRequest(new { status = "fail", message = "登录失败，请检查账号和密码是否正确" });
                }
            }
            else
            {
                Console.WriteLine("3");
                // 模型验证失败，返回错误信息
                return BadRequest(ModelState);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] TodoItem user)
        {
            if (ModelState.IsValid)
            {
                var todo = todoDb.TodoItems.FirstOrDefault(t => t.Id == user.Id);
                if (todo == null)
                {
                    Console.WriteLine("1");
                    // 没有找到该用户，可以注册
                    todo = new TodoItem
                    {
                        Id = user.Id,
                        Password = user.Password,
                        Nickname = user.Nickname
                    };
                    todoDb.TodoItems.Add(todo);
                    todoDb.SaveChanges();

                    // 注册成功，返回用户信息
                    return Ok(new { statusCode = 200, user = todo });
                }
                else
                {
                    Console.WriteLine("2");
                    // 找到该用户，注册失败
                    return BadRequest(new { status = "fail", message = "该账号已存在" });
                }
            }
            else
            {
                Console.WriteLine("3");
                // 模型验证失败，返回错误信息
                return BadRequest(ModelState);
            }
        }

        [HttpPost("createCashFlow")]
        public IActionResult CreateCashFlow([FromBody] CashFlow cashFlow)
        {
            if (ModelState.IsValid)
            {
                // 生成唯一的id
                string id = Guid.NewGuid().ToString();
                // 为新增的现金流设定唯一ID
                cashFlow.Id = id;

                Console.WriteLine($"CashFlow Id={cashFlow.Id}, Type={cashFlow.Type}, Amount={cashFlow.Amount}, CategoryId={cashFlow.CategoryId}, Date={cashFlow.Date}, Remark={cashFlow.Remark}, Image={cashFlow.Image}");


                // 将现金流存储到数据库中
                todoDb.CashFlows.Add(cashFlow);
                todoDb.SaveChanges();
                // 使用匿名对象返回新增的现金流数据
                return Ok(new { code = 0, message = "新增成功", data = cashFlow });
            }
            else
            {
                // 返回错误状态码 1 和模型验证失败的信息
                return BadRequest(new { code = 1, message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage });
            }
        }
        public class CashFlowRequest
        { 
            public int Year { get; set; }   
            public int Month { get; set; }
        }

        [HttpPost("getCashFlowList")]
        public ActionResult<object> GetCashFlowList([FromBody]CashFlowRequest requestData)
        {
             int year=requestData.Year;
            int month=requestData.Month;

            string dateString = $"{year}-{month:00}";
            List<CashFlow> cashFlowList = todoDb.CashFlows
                .Where(c => c.Date.StartsWith(dateString))
                .OrderByDescending(c => c.Date)
                .ToList();

            // 构造包含时间、星期、收入、支出等属性的列表
            // 构造包含时间、星期、收入、支出等属性的列表
            var resultList = cashFlowList.Select(c => new {
                time = c.Date,
                inp = c.Type == 10 ? c.Amount : 0,
                outp = c.Type == 20 ? c.Amount : 0,
                icon = c.Image,
                category_id = c.CategoryId,
                remark = c.Remark,
                type = c.Type,
                amount = c.Amount,
                Id = c.Id,
            }).ToList();


            // 计算总收入和总支出
            decimal totalIncome = cashFlowList.Where(c => c.Type == 10).Sum(c => c.Amount);
            decimal totalExpense = cashFlowList.Where(c => c.Type == 20).Sum(c => c.Amount);

            // 返回现金流列表和总收入、总支出
            return new
            {
                code = 200,
                msg = "success",
                data = new
                {
                    list = resultList,
                    income = totalIncome,
                    outcome = totalExpense
                }
            };
        }

        [HttpGet("getStatisticData")]
        public async Task<ActionResult> GetStatisticData([FromQuery] int year, int type, int? month, string query_type)
        {

            var query = todoDb.CashFlows
            .Where(cf => cf.Type == type).ToList();

            var result = query.Where(cf => cf.Type == type && query_type == "year" ? cf.Year.Year == year : cf.Month.Month == month)
            .GroupBy(cf => cf.CategoryId)
            .Select(g => new
            {
                Name = g.Key,
                Value = g.Sum(cf => cf.Amount)
            }).ToList();


            var zhu = todoDb.CashFlows.ToList();
            var zhu2 = zhu.ToList();

            var categories = zhu2.Select(x => x.DateTime.Year + "-" + x.DateTime.Month).Distinct().ToList();
            List<Servicess> servicesses = new List<Servicess>();
            Servicess outs = new Servicess();
            outs.Name = "支出";
            outs.Data = new List<decimal>();
            Servicess inputs = new Servicess();
            inputs.Name = "收入";
            inputs.Data = new List<decimal>();

            foreach (var item in categories)
            {

                outs.Data.Add(zhu2.Where(x => x.DateTime.Year + "-" + x.DateTime.Month == item && x.Type == 20).Sum(x => x.Amount));
                inputs.Data.Add(zhu2.Where(x => x.DateTime.Year + "-" + x.DateTime.Month == item && x.Type == 10).Sum(x => x.Amount));
            }



            servicesses.Add(outs);
            servicesses.Add(inputs);

            var chartData = new
            {
                categories,
                series = servicesses
            };

            return Ok(new
            {
                success = true,
                data = result,
                chartData
    
            });
        }
        [HttpPost("findbill")]
        public ActionResult<object> FindBill()
        {
            var dateList = todoDb.CashFlows.Select(c => c.Date).Distinct().ToList();
            int totalDays = dateList.Count();
            decimal totalAmount = todoDb.CashFlows.Sum(c => c.Amount);
            int usedDays = CalculateUsedDays(todoDb.CashFlows.ToList());

            return new
            {
                TotalDays = totalDays,
                TotalCount = todoDb.CashFlows.Count(),
                UsedDays = usedDays
            };
            int CalculateUsedDays(List<CashFlow> cashFlows)
            {
                int usedDays = 0;
                DateTime currentDate = DateTime.Now;
                var groups = cashFlows.GroupBy(c => c.Date);
                if (groups.Count() == 0)
                {
                    return 0;
                }
                string earlistDateStr = groups.OrderBy(g => g.Key).First().Key;
                DateTime earlistDate = DateTime.Parse(earlistDateStr);
               
                    usedDays = (int)(currentDate - earlistDate).TotalDays + 1;
                
                return usedDays;
            }
        }
        public class getString
        {
            public string cashFlowId { get; set; }
        }
        [HttpPost("getCashFlowInfo")]
        public ActionResult<object> GetCashFlowInfo([FromBody] getString reData)
        {
            // 根据现金流ID从数据库中查询对应的现金流记录
            string cashFlowId = reData.cashFlowId;
            var cashFlow = todoDb.CashFlows.FirstOrDefault(c => c.Id == cashFlowId);
            List<CashFlow> cashFlowList = todoDb.CashFlows
                .ToList();

            var resultList = cashFlowList.Select(c => new
            {
                time = c.Date,
                inp = c.Type == 10 ? c.Amount : 0,
                outp = c.Type == 20 ? c.Amount : 0,
                icon = c.Image,
                category_id = c.CategoryId,
                remark = c.Remark,
                type = c.Type,
                amount = c.Amount,
                Id = c.Id,
            }).ToList();
            return new
            {
                code=200,
                msg="success",
                data=new
                {
                    list = resultList
                }
            };
        }



        public class delServive
        {
            public string cashFlowIdDel { get; set; }
        }

        [HttpPost("delCashFlow")]
        public ActionResult<object> DeleteCashFlow(delServive reData)
        {
            // 根据现金流ID从数据库中查询对应的现金流记录
            string cashFlowId=reData.cashFlowIdDel;
            var cashFlow = todoDb.CashFlows.FirstOrDefault(c => c.Id == cashFlowId);

            if (cashFlow != null)
            {
                try
                {
                    // 从数据库中移除现金流记录
                    todoDb.CashFlows.Remove(cashFlow);
                    todoDb.SaveChanges();

                    // 删除成功，返回结果
                    return Ok(new { code = 0, message = "删除成功", data = cashFlow, result = "Success" });

                }
                catch (Exception ex)
                {
                    // 删除过程中出现异常
                    return BadRequest(new { code = 1, message = "删除失败", error = ex.Message });
                }
            }
            else
            {
                // 找不到指定的现金流记录
                return NotFound(new { code = 1, message = "找不到指定的现金流记录" });
            }
        }

        public class editServiceData
        {
            public CashFlow updatedData { get; set; }
public string cashFlowId { get; set; }
            
        }
        [HttpPost("editCashFlow")]
        public ActionResult<object> EditCashFlow(editServiceData requestData)
        {
            // 根据现金流ID从数据库中查询对应的现金流记录
            string cashFlowId = requestData.cashFlowId;
            var cashFlow = todoDb.CashFlows.FirstOrDefault(c => c.Id == cashFlowId);

            if (cashFlow != null)
            {
                try
                {
                    // 更新现金流记录
                    cashFlow.Type = requestData.updatedData.Type;
                    cashFlow.Amount = requestData.updatedData.Amount;
                    cashFlow.CategoryId = requestData.updatedData.CategoryId;
                    cashFlow.Date = requestData.updatedData.Date;
                    cashFlow.Remark = requestData.updatedData.Remark;
                    cashFlow.Image = requestData.updatedData.Image;

                    todoDb.SaveChanges();

                    // 编辑成功，返回结果
                    return Ok(new { code = 0, message = "编辑成功", data = cashFlow });
                }
                catch (Exception ex)
                {
                    // 编辑过程中出现异常
                    return BadRequest(new { code = 1, message = "编辑失败", error = ex.Message });
                }
            }
            else
            {
                // 找不到指定的现金流记录
                return NotFound(new { code = 1, message = "找不到指定的现金流记录" });
            }
        }


    }

    public class Servicess
    {
        public string Name { get; set; }

        public List<decimal> Data { get; set; }
    }

}