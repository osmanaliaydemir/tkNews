import React from 'react';
import { Outlet } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../store';
import Navbar from '../components/Navbar';
import Sidebar from '../components/Sidebar';
import Toast from '../components/Toast';

const Layout: React.FC = () => {
  const { isDarkMode } = useSelector((state: RootState) => state.ui);

  return (
    <div className={`min-h-screen bg-gray-50 ${isDarkMode ? 'dark' : ''}`}>
      <Navbar />
      
      <div className="flex h-[calc(100vh-4rem)]">
        <Sidebar />
        
        <main className="flex-1 overflow-auto">
          <div className="container mx-auto px-4 py-8">
            <Outlet />
          </div>
        </main>
      </div>
      
      <Toast />

      <footer className="bg-white shadow-sm dark:bg-gray-800">
        <div className="container mx-auto py-4 px-4">
          <p className="text-center text-sm text-gray-500 dark:text-gray-400">
            © {new Date().getFullYear()} tkNews. All rights reserved.
          </p>
        </div>
      </footer>
    </div>
  );
};

export default Layout; 