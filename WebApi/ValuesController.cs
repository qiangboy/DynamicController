namespace WebApi
{
    /// <summary>
    /// Description: 用户服务
    /// Created by: 郑守强
    /// Created on: 2023/10/25 9:55:35
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] CreateUserDto input)
        {
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound("");
            }

            return Ok("");
        }
    }
}
