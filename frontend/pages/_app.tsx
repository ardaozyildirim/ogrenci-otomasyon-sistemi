import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { useRouter } from "next/router";
import { useEffect } from "react";

export default function App({ Component, pageProps }: AppProps) {
  const router = useRouter();

  useEffect(() => {
    // List of public routes that don't require authentication
    const publicRoutes = ["/login", "/register"];
    const token = localStorage.getItem("token");
    
    // If there's no token and we're not on a public route, redirect to login
    if (!token && !publicRoutes.includes(router.pathname)) {
      router.push("/login");
    }
    
    // If there's a token and we're on a public route, redirect to dashboard
    if (token && publicRoutes.includes(router.pathname)) {
      router.push("/dashboard");
    }
  }, [router]);

  return <Component {...pageProps} />;
}