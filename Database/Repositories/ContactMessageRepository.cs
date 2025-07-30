using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class ContactMessageRepository
{
    private readonly AppDbCtx _dbCtx;
    
    public ContactMessageRepository(AppDbCtx dbCtx)
    {
        _dbCtx = dbCtx;
    }
    
    public async Task<List<ContactMessage>> GetContactMessages()
    {
        return await _dbCtx.ContactMessages
            .ToListAsync();
    }
    
    public async Task<ContactMessage?> GetContactMessageById(int id)
    {
        return await _dbCtx.ContactMessages
            .FirstOrDefaultAsync(cm => cm.Id == id);
    }
    
    public async Task AddContactMessage(ContactMessage contactMessage)
    {
        _dbCtx.ContactMessages.Add(contactMessage);
        await _dbCtx.SaveChangesAsync();
    }
    
    public async Task UpdateContactMessage(ContactMessage contactMessage)
    {
        _dbCtx.ContactMessages.Update(contactMessage);
        await _dbCtx.SaveChangesAsync();
    }
    
    public async Task DeleteContactMessage(int id)
    {
        var contactMessage = await GetContactMessageById(id);
        if (contactMessage != null)
        {
            _dbCtx.ContactMessages.Remove(contactMessage);
            await _dbCtx.SaveChangesAsync();
        }
    }
}