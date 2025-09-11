# Student Management System - Frontend

This is the frontend application for the Student Management System, built with Next.js. It provides a user interface for managing students, courses, grades, and attendance.

## Features

- User authentication (login/logout)
- Dashboard with overview of grades and attendance
- Grade management (view grades)
- Attendance management (view attendance records)
- Course management (to be implemented)
- Student management (to be implemented)

## Tech Stack

- [Next.js](https://nextjs.org) - React framework for production
- TypeScript - Typed JavaScript
- Tailwind CSS - Utility-first CSS framework

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

The frontend application connects to the backend API running on `http://localhost:5000`. Make sure the backend is running before using the frontend.

## Project Structure

- `components/` - Reusable UI components
- `pages/` - Next.js pages
- `services/` - API service layer
- `utils/` - Utility functions
- `styles/` - Global styles

## Environment Variables

The application uses the following environment variables:

- `NEXT_PUBLIC_API_BASE_URL` - The base URL for the backend API (default: http://localhost:5000/api)

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn-pages-router) - an interactive Next.js tutorial.

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/pages/building-your-application/deploying) for more details.