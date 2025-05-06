
@echo off
echo ========================================
echo 💡 Improved GitHub Push Script (SEC Branch)
echo ========================================

:: Set Git global config (optional)
git config --global core.autocrlf true

:: Step 1: Ensure you're in the repo root
echo 📁 Current directory: %cd%

:: Step 2: Initialize Git repo if not already
if not exist .git (
    echo 🧱 Initializing new Git repository...
    git init
)

:: Step 3: Set remote if not already set
git remote -v | findstr "origin"
if %errorlevel% neq 0 (
    echo 🔗 Adding remote origin...
    git remote add origin https://github.com/Mian-Muhammad-Bilal/CyberED.git
)

:: Step 4: Clean up old history and remove large files
echo 🧹 Removing large files from history...
git filter-repo --force --path "Assets/Firebase/Plugins/x86_64/FirebaseCppApp-12_8_0.bundle" --invert-paths
git filter-repo --force --path "Assets/Firebase/Plugins/x86_64/FirebaseCppApp-12_8_0.so" --invert-paths

:: Step 5: Garbage collect unnecessary data
rd /s /q .git\refs\original 2>nul
git reflog expire --expire=now --all
git gc --prune=now

:: Step 6: Track large files using Git LFS
echo 📦 Setting up Git LFS...
git lfs install
git lfs track "*.bundle"
git lfs track "*.so"
git add .gitattributes

:: Step 7: Stage all changes
echo 🗂️ Adding all files to staging...
git add -A

:: Step 8: Commit changes
echo 📝 Committing changes...
git commit -m "Full repo upload to SEC branch with Git LFS support" || echo Skipping commit (nothing new)

:: Step 9: Switch or create SEC branch
echo 🌿 Switching to SEC branch...
git checkout -B SEC

:: Step 10: Force push to GitHub
echo 🚀 Pushing all changes to GitHub (branch: SEC)...
git push origin SEC --force

echo ========================================
echo ✅ All done! Your repo is live on GitHub!
echo ========================================
pause
