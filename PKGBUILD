# Maintainer: PatzminiHD  <youremail@domain.com>
pkgname=bsprog
pkgver=1.1.1
pkgrel=1
pkgdesc="BadSyncProgram. Sync two local folders"
arch=('x86_64')
url="https://github.com/PatzminiHD/BadSyncProgram"
license=('GPL3')
depends=(
    "icu"
    "zlib"
)
makedepends=(
    "git"
    "dotnet-host"
    "dotnet-sdk-6.0"
)
options=("staticlibs" "!strip")
# source=("${url}/releases/download/v${pkgver}/bsprog_${pkgver}_x86_64.tar.gz")
source=("git+${url}.git")
sha512sums=("SKIP")

build() {
  cd "BadSyncProgram"

  MSBUILDDISABLENODEREUSE=1 dotnet publish --self-contained --runtime linux-x64 --output ../$pkgname
}

package() {
#  cd "BadSyncProgram"

  install -d $pkgdir/opt/
  install -d $pkgdir/usr/bin/

  cp -r $pkgname "$pkgdir/opt/$pkgname/"
  mv "$pkgdir/opt/$pkgname/BadSyncProgram" "$pkgdir/opt/$pkgname/$pkgname"
  rm "$pkgdir/opt/$pkgname/BadSyncProgram.pdb"
  ln -s "$pkgdir/opt/$pkgname/$pkgname" "$pkgdir/usr/bin/$pkgname"
}
