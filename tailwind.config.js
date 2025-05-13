/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}", // tells Tailwind to scan these files for class names
  ],
  theme: {
    extend: {}, // you can extend the default Tailwind theme here
  },
  plugins: [], // add Tailwind plugins here if needed
}
