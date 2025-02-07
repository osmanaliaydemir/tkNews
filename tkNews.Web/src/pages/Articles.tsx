import React from 'react';
import { useGetArticlesQuery } from '../features/articles/articlesApi';
import { Link } from 'react-router-dom';

const Articles: React.FC = () => {
  const { data: articles, isLoading, error } = useGetArticlesQuery();

  if (isLoading) {
    return <div className="flex justify-center items-center min-h-screen">Loading...</div>;
  }

  if (error) {
    return <div className="flex justify-center items-center min-h-screen">Error loading articles</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Articles</h1>
        <Link
          to="/articles/new"
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700"
        >
          Create New Article
        </Link>
      </div>
      <div className="grid grid-cols-1 gap-6">
        {articles?.map((article: any) => (
          <div key={article.id} className="bg-white rounded-lg shadow-md p-6">
            <h2 className="text-2xl font-semibold mb-4">{article.title}</h2>
            <p className="text-gray-600 mb-4">{article.summary}</p>
            <div className="flex justify-between items-center">
              <div className="text-sm text-gray-500">
                <span>Published: {new Date(article.publishedAt).toLocaleDateString()}</span>
                <span className="mx-2">•</span>
                <span>Author: {article.author}</span>
              </div>
              <Link
                to={`/articles/${article.slug}`}
                className="text-blue-600 hover:text-blue-800"
              >
                Read More
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Articles; 