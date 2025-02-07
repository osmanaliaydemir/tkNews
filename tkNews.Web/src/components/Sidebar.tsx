import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../store';
import {
  HomeIcon,
  NewspaperIcon,
  UserGroupIcon,
  TagIcon,
  Cog6ToothIcon,
  UserIcon,
  PlusIcon,
} from '@heroicons/react/24/outline';

const navigation = [
  { name: 'Ana Sayfa', href: '/', icon: HomeIcon },
  { name: 'Makaleler', href: '/articles', icon: NewspaperIcon },
  { name: 'Yazarlar', href: '/authors', icon: UserGroupIcon },
  { name: 'Etiketler', href: '/tags', icon: TagIcon },
];

const adminNavigation = [
  { name: 'Ayarlar', href: '/settings', icon: Cog6ToothIcon },
];

const Sidebar: React.FC = () => {
  const { isAuthenticated } = useSelector((state: RootState) => state.auth);
  const location = useLocation();

  const isActiveLink = (path: string) => {
    return location.pathname === path;
  };

  return (
    <div className="w-64 bg-white dark:bg-gray-800 border-r border-gray-200 dark:border-gray-700">
      <div className="h-full flex flex-col">
        <div className="flex-1 overflow-y-auto">
          <nav className="px-4 py-4 space-y-1">
            {navigation.map((item) => {
              const isActive = isActiveLink(item.href);
              return (
                <Link
                  key={item.name}
                  to={item.href}
                  className={`flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-colors ${
                    isActive
                      ? 'bg-blue-50 text-blue-700 dark:bg-blue-900/50 dark:text-blue-200'
                      : 'text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700'
                  }`}
                >
                  <item.icon className={`h-5 w-5 mr-3 ${
                    isActive ? 'text-blue-700 dark:text-blue-200' : 'text-gray-400 dark:text-gray-500'
                  }`} />
                  {item.name}
                </Link>
              );
            })}

            {isAuthenticated && (
              <div className="pt-4 mt-4 border-t border-gray-200 dark:border-gray-700 space-y-1">
                <Link
                  to="/profile"
                  className={`flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-colors ${
                    isActiveLink('/profile')
                      ? 'bg-blue-50 text-blue-700 dark:bg-blue-900/50 dark:text-blue-200'
                      : 'text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700'
                  }`}
                >
                  <UserIcon className={`h-5 w-5 mr-3 ${
                    isActiveLink('/profile') ? 'text-blue-700 dark:text-blue-200' : 'text-gray-400 dark:text-gray-500'
                  }`} />
                  Profil
                </Link>
                <Link
                  to="/articles/new"
                  className={`flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-colors ${
                    isActiveLink('/articles/new')
                      ? 'bg-blue-50 text-blue-700 dark:bg-blue-900/50 dark:text-blue-200'
                      : 'text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700'
                  }`}
                >
                  <PlusIcon className={`h-5 w-5 mr-3 ${
                    isActiveLink('/articles/new') ? 'text-blue-700 dark:text-blue-200' : 'text-gray-400 dark:text-gray-500'
                  }`} />
                  Yeni Makale
                </Link>
              </div>
            )}
          </nav>
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
