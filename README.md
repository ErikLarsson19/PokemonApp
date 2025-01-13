# Pokemon Card Social Media Bot 🎴

## Overview
An automated social media bot built in C# that posts Pokemon card information to Twitter and Bluesky. The bot features scheduled posts about high-value cards from current sets and throwback cards from classic sets, providing market price analysis and nostalgic content for Pokemon card collectors.

## Features 🌟
- **Automated Social Media Posting**
  - Simultaneous posting to Twitter (X) and Bluesky
  - Scheduled posts every 10-12 hours
  - Alternating content types for variety

- **Content Types**
  - High-value card spotlights with current market prices
  - Throwback cards from classic sets
  - Dynamic message templates for engaging content
  - Optimized image processing for each platform

- **Smart Card Selection**
  - Random set selection algorithm
  - Top percentile filtering for high-value cards
  - Classic set filtering for throwback posts
  - Real-time market price data

## Technical Architecture 🏗
The project follows a three-layer architecture for clean separation of concerns:

### 1. Presentation Layer (PL)
- Background service for scheduling
- Task coordination and management
- Environment configuration

### 2. Business Logic Layer (BLL)
- Task handlers for different post types
- Image processing and optimization
- Post content generation
- Set randomization logic

### 3. Data Access Layer (DAL)
- Pokemon TCG API integration
- Supabase database operations
- Data models and entities

## Technology Stack 💻
- **Framework:** C# / .NET
- **External APIs:**
  - Pokemon TCG API
  - Twitter API (X)
  - Bluesky API (ATProtocol)
- **Database:** Supabase (PostgreSQL)
- **Libraries:**
  - ImageSharp - Image processing
  - FishyFlip - Bluesky integration
  - LinqToTwitter - Twitter integration

## Setup Requirements 🔧
1. Clone the repository
2. Set up the following environment variables:

```bash
# Social Media Credentials
BLUESKY_IDENTIFIER=your_identifier
BLUESKY_PASSWORD=your_password
TWITTER_APIKEY=your_key
TWITTER_APIKEYSECRET=your_secret
TWITTER_ACCESSTOKEN=your_token
TWITTER_ACCESSTOKENSECRET=your_token_secret

# Pokemon TCG API
TCG_APIKEY=your_api_key

# Supabase Database
SUPABASE_KEY=your_key
SUPABASE_URL=your_url