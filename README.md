
# 🔷 Elektrotehnički fakultet – Univerzitet u Banjoj Luci  
## 📘 KORISNIČKO UPUTSTVO

**Naziv aplikacije:** FIT – Aplikacija za praćenje kalorija  
**Autor:** Milan Aleksić  
**Studijski program:** Softversko inženjerstvo  
**Godina izrade:** 2025.

---

## 📑 Sadržaj

1. [📘 Uvod](#1-📘-uvod)  
2. [🧭 Korišćenje aplikacije](#2-🧭-korišćenje-aplikacije)  
   - [🔐 Prijava i registracija](#21-🔐-prijava-i-registracija)  
   - [🍽️ Unos obroka, aktivnosti i cilja](#22-🍽️-unos-obroka-aktivnosti-i-cilja)  
   - [📊 Bilans kalorija](#23-📊-bilans-kalorija)  
   - [🎨 Opcije prikaza](#24-🎨-opcije-prikaza)  
3. [👤 Uloge korisnika](#3-👤-uloge-korisnika)  
4. [🧰 Tehnologije korištene u izradi](#4-🧰-tehnologije-korištene-u-izradi)  
5. [📬 Kontakt](#5-📬-kontakt)  

---

## 1. 📘 Uvod

Dobrodošli u korisničko uputstvo za aplikaciju **Fit**.  
Aplikacija je razvijena kao dio projektnog rada na predmetu Interakcija čovjek-računar (Elektrotehnički fakultet, Univerzitet u Banjoj Luci).  
Osnovna namjena aplikacije je da pomogne korisnicima da prate svoj dnevni unos kalorija kroz obroke i aktivnosti, te održe zdraviji način života.

---

## 2. 🧭 Korišćenje aplikacije

### 2.1 🔐 Prijava i registracija

Po pokretanju aplikacije otvara se forma za **prijavu**.  
- Ako korisnik nema nalog, klikom na **Registruj se** otvara se forma za unos korisničkog imena i lozinke, kao i samog imena i prezimena.
- Lozinka se bezbedno čuva heširanjem.

Uspješnom prijavom korisnik se preusmjerava na odgovarajući interfejs u zavisnosti od uloge.

<p align="center">
  <img src="images/pocetni_ekran.png" alt="Početni ekran - prijava" width="700"/>
</p>

<p align="center">
  <img src="images/korisnik_prikaz.png" alt="Prikaz za korisnika" width="700"/>
</p>

---

### 2.2 🍽️ Unos obroka, aktivnosti i cilja

Korisnik može da unosi:

#### ✅ Obroci (hrana):
- Naziv namirnice
- Masa (u gramima)
- Kalorije
- **Makronutrijenti**: ugljeni hidrati, proteini, masti
- Tip obroka (doručak, ručak, užina itd.)
- Datum i vrijeme unosa

<p align="center">
  <img src="images/unos_obroka.png" alt="Unos obroka" width="700"/>
</p>

#### ✅ Aktivnosti:
- Tip aktivnosti (hodanje, trčanje, plivanje itd.)
- Trajanje u minutama
- Datum i vrijeme aktivnosti

<p align="center">
  <img src="images/unos_aktivnosti.png" alt="Unos aktivnosti" width="700"/>
</p>

#### 🎯 Ciljani dnevni unos kalorija:
Korisnik može unijeti **lični cilj** – broj kalorija koji planira da unese tokom dana.  
Ovaj cilj se koristi za poređenje u bilansu kalorija.

<p align="center">
  <img src="images/cilj.png" alt="Unos cilja kalorija" width="700"/>
</p>

Sve stavke se prikazuju u tabelama sa mogućnošću:
- Dodavanja ✅  
- Izmjene ✏️  
- Brisanja 🗑️  

Tabele omogućavaju filtriranje po nazivu i tipu radi lakšeg pretraživanja.

Korisnik može pregledati **sve prethodno unesene obroke i aktivnosti**, sortirane po datumu.

<p align="center">
  <img src="images/pregled_obroka.png" alt="Pregled unosa obroka" width="700"/>
</p>

<p align="center">
  <img src="images/pregled_aktivnosti.png" alt="Pregled aktivnosti" width="700"/>
</p>

---

### 2.3 📊 Bilans kalorija

U bilansu se automatski izračunava:

- ✅ **Ukupan unos kalorija** (zbir kalorija iz obroka)
- ✅ **Ukupna potrošnja kalorija** (na osnovu trajanja i vrste aktivnosti)
- ✅ **Uneseni cilj** (ako je korisnik prethodno definisao dnevni limit)
- ✅ **Preostale kalorije**: razlika između cilja i bilansa

Bilans može biti:
- **Pozitivan** – korisnik je unio više nego što je potrošio
- **Negativan** – korisnik je potrošio više kalorija
- **Neutralan** – unos i potrošnja su izbalansirani

Prikaz bilansa je kombinovan:
- Numerički (tačne vrijednosti)
- Vizualno (boje)

<p align="center">
  <img src="images/bilans.png" alt="Prikaz bilansa kalorija" width="700"/>
</p>

---

### 2.4 🎨 Opcije prikaza

Aplikacija podržava sljedeće opcije:
- Promjena teme (svijetla, tamna, šarena)
- Promjena jezika (Srpski / Engleski)

---

## 3. 👤 Uloge korisnika

### ➤ Korisnik:

- Unosi i uređuje:
  - **Obroke i makronutrijente**
  - **Fizičke aktivnosti**
  - **Ciljani broj kalorija** (dnevni limit)
- Ima pregled svih svojih:
  - Unosa hrane
  - Unosa aktivnosti
  - Dnevnog bilansa kalorija
- Može mijenjati temu i jezik interfejsa

### ➤ Administrator:

- Ima pristup svim korisničkim nalozima
- Može:
  - **Dodavati nove korisnike**
  - **Izmjenjivati korisničke podatke**
  - **Brisati korisnike**
- Može pregledati sve podatke vezane za korisnike

<p align="center">
  <img src="images/pregled_korisnika.png" alt="Pregled korisnika" width="700"/>
</p>

<p align="center">
  <img src="images/admin_prikaz.png" alt="Administrator prikaz" width="700"/>
</p>

---

## 4. 🧰 Tehnologije korištene u izradi

- **WPF (Windows Presentation Foundation)**
- **C#**
- **XAML**
- **MySQL**
- **Entity Framework Core**
- **Material Design in XAML Toolkit** – moderan vizuelni prikaz

---

## 5. 📬 Kontakt

Za dodatne informacije ili tehničku podršku:

📧 **milan2001alexic@hotmail.com**  
🔗 GitHub: [https://github.com/Alexic11/fit-kalorije-app](https://github.com/Alexic11/fit-kalorije-app)
