@echo off
echo ----------------------------------------
echo Git LFS Firebase Fix Script (Branch: SEC)
echo ----------------------------------------

:: Step 1: Initialize Git LFS
git lfs install

:: Step 2: Track large file types
git lfs track "*.bundle"
git lfs track "*.so"

:: Step 3: Remove large files from Git index only (not from disk)
git rm --cached "Assets/Firebase/Plugins/x86_64/FirebaseCppApp-12_8_0.bundle"
git rm --cached "Assets/Firebase/Plugins/x86_64/FirebaseCppApp-12_8_0.so"

:: Step 4: Re-add files (now tracked by LFS)
git add .
git commit -m "Moved Firebase .bundle and .so files to Git LFS"

:: Step 5: Push to the SEC branch
git push origin SEC

echo ----------------------------------------
echo ✅ Done! Pushed to 'SEC' branch using Git LFS.
echo ----------------------------------------

pause
