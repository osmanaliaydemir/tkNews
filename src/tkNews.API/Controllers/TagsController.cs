using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Services;
using tkNews.Domain.Entities;

namespace tkNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    
    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetAll()
    {
        var tags = await _tagService.GetAllAsync();
        return Ok(tags);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Tag>> GetById(Guid id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null) return NotFound();
        return Ok(tag);
    }
    
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<Tag>> GetBySlug(string slug)
    {
        var tag = await _tagService.GetBySlugAsync(slug);
        if (tag == null) return NotFound();
        return Ok(tag);
    }
    
    [HttpGet("popular/{count:int}")]
    public async Task<ActionResult<IEnumerable<Tag>>> GetPopular(int count)
    {
        var tags = await _tagService.GetPopularTagsAsync(count);
        return Ok(tags);
    }
    
    [HttpGet("article/{articleId:guid}")]
    public async Task<ActionResult<IEnumerable<Tag>>> GetByArticle(Guid articleId)
    {
        var tags = await _tagService.GetTagsByArticleIdAsync(articleId);
        return Ok(tags);
    }
    
    [HttpPost]
    public async Task<ActionResult<Tag>> Create(Tag tag)
    {
        var createdTag = await _tagService.CreateTagAsync(tag);
        return CreatedAtAction(nameof(GetById), new { id = createdTag.Id }, createdTag);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Tag tag)
    {
        if (id != tag.Id) return BadRequest();
        
        var existingTag = await _tagService.GetByIdAsync(id);
        if (existingTag == null) return NotFound();
        
        await _tagService.UpdateTagAsync(tag);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null) return NotFound();
        
        await _tagService.DeleteTagAsync(id);
        return NoContent();
    }
    
    [HttpGet("{id:guid}/article-count")]
    public async Task<ActionResult<int>> GetArticleCount(Guid id)
    {
        var count = await _tagService.GetArticleCountAsync(id);
        return Ok(count);
    }
} 