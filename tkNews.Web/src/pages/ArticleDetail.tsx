import React from 'react';
import { useParams, Link } from 'react-router-dom';
import { useGetArticleByIdQuery } from '../features/articles/articlesApi';

const ArticleDetail: React.FC = () => {
  const { slug } = useParams<{ slug: string }>();
  const { data: article, isLoading, error } = useGetArticleByIdQuery(slug);

  if (isLoading) {
    return <div className="flex justify-center items-center min-h-screen">Loading...</div>;
  }

  if (error) {
    return <div className="flex justify-center items-center min-h-screen">Error loading article</div>;
  }

  if (!article) {
    return <div className="flex justify-center items-center min-h-screen">Article not found</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <Link to="/articles" className="text-blue-600 hover:text-blue-800 mb-8 inline-block">
        ← Back to Articles
      </Link>
      
      <article className="max-w-4xl mx-auto">
        {article.imageUrl && (
          <img
            src={article.imageUrl}
            alt={article.title}
            className="w-full h-96 object-cover rounded-lg mb-8"
          />
        )}
        
        <h1 className="text-4xl font-bold mb-4">{article.title}</h1>
        
        <div className="flex items-center text-gray-600 mb-8">
          <span>By {article.author}</span>
          <span className="mx-2">•</span>
          <span>{new Date(article.publishedAt).toLocaleDateString()}</span>
        </div>
        
        <div className="prose prose-lg max-w-none">
          {article.content}
        </div>
      </article>
    </div>
  );
};

export default ArticleDetail; 