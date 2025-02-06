namespace tkNews.Domain.Enums;

[Flags]
public enum Permissions
{
    None = 0,
    
    // Article Permissions
    ViewArticles = 1 << 0,
    CreateArticle = 1 << 1,
    EditArticle = 1 << 2,
    DeleteArticle = 1 << 3,
    PublishArticle = 1 << 4,
    
    // Category Permissions
    ViewCategories = 1 << 5,
    ManageCategories = 1 << 6,
    
    // Comment Permissions
    ViewComments = 1 << 7,
    CreateComment = 1 << 8,
    ModerateComments = 1 << 9,
    
    // Author Permissions
    ViewAuthors = 1 << 10,
    ManageAuthors = 1 << 11,
    
    // Tag Permissions
    ViewTags = 1 << 12,
    ManageTags = 1 << 13,
    
    // User Management
    ViewUsers = 1 << 14,
    ManageUsers = 1 << 15,
    
    // Role Management
    ViewRoles = 1 << 16,
    ManageRoles = 1 << 17,
    
    // System Settings
    ViewSettings = 1 << 18,
    ManageSettings = 1 << 19,
    
    // Predefined Role Permissions
    User = ViewArticles | ViewCategories | ViewComments | CreateComment | ViewAuthors | ViewTags,
    
    Author = User | CreateArticle | EditArticle | ViewSettings,
    
    Editor = Author | DeleteArticle | PublishArticle | ModerateComments | ManageCategories | ManageTags,
    
    Admin = Editor | ManageAuthors | ManageUsers | ManageRoles | ManageSettings
} 